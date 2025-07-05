using System.Text;
using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Invoicing.Mapping;
using Eu.Iamia.Utils.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.Application;

public interface IExportService
{
    Task<ExecutionStatus> ExportBookedInvoices(
        IInterval<DateTime> dateInterval,
        bool includeNonInvoicedCustomers,
        FileInfo outputFile,
        CancellationToken cancellationToken
    );
}

public class ExportService : IExportService
{
    private readonly SettingsForInvoicingApplication _invoicingApplicationSettings;
    private readonly IMappingService _mappingService;
    private readonly IEconomicGatewayV2 _economicGateway;

    private readonly List<ExportData> _exportData = new List<ExportData>();

    internal ExportService(
        SettingsForInvoicingApplication invoicingApplicationSettings,
        IMappingService mappingService,
        IEconomicGatewayV2 economicGateway
    )
    {
        _invoicingApplicationSettings = invoicingApplicationSettings;
        _mappingService = mappingService;
        _economicGateway = economicGateway;
    }

    public ExportService(
        IOptions<SettingsForInvoicingApplication> invoicingApplicationOptions,
        IMappingService mappingService,
        IEconomicGatewayV2 economicGateway
    ) : this(invoicingApplicationOptions.Value, mappingService, economicGateway)
    { }

    /// <summary>
    /// Updates <em>_exportData</em> with invoice lines.
    /// </summary>
    /// <param name="dateInterval"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task LoadInvoices(
        IInterval<DateTime> dateInterval,
        CancellationToken cancellationToken
    )
    {
        await _mappingService.LoadProductCache();

        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var bookedInvoicesHandle = await _economicGateway.GetBookedInvoices(page, 20, dateInterval);

            foreach (var inv in bookedInvoicesHandle.Invoices)
            {
                var bi = await _economicGateway.GetBookedInvoice(inv.bookedInvoiceNumber);
                foreach (var line in bi.lines)
                {
                    Console.Write(".");
                    var product = _mappingService.ProductDtoCache.FirstOrDefault(prod => prod.ProductNumber == line.product.productNumber)
                                  ?? new ProductDto
                                  {
                                      ProductNumber = line.product.productNumber,
                                      Name = $"Product {line.product.productNumber} not found!"
                                  };

                    var ed = new ExportData
                    {
                        Address = (inv.recipient.address ?? "").Replace("\n", ", "),
                        CustomerNumber = inv.customer.customerNumber,
                        InvoiceDate = inv.date,
                        InvoiceNumber = inv.bookedInvoiceNumber,
                        Name = inv.recipient.name,
                        Price = line.unitNetPrice,
                        ProductName = product.Name,
                        ProductNumber = int.Parse(product.ProductNumber),
                        Quantity = line.quantity,
                        TotalAmount = line.totalNetAmount
                    };
                    _exportData.Add(ed);
                }
            }

            @continue = bookedInvoicesHandle.Invoices.Any() && page < 100;
            page++;
        }
    }

    /// <summary>
    /// Updates <em>_exportData</em> with empty invoice lines.
    /// </summary>
    private async Task MergeCustomerDetails(ICollection<int> customerGroupsToAccept, bool includeNonInvoicedCustomers)
    {
        await _mappingService.LoadCustomerCache();

        foreach (var customer in _mappingService.CustomerDtoCache)
        {
            var existing = _exportData.Where(ed => ed.CustomerNumber == customer.CustomerNumber).ToList();
            foreach (var line in existing)
            {
                line.CustomerGroupNumber = customer.CustomerGroupNumber;
            }

            if (!includeNonInvoicedCustomers)
            {
                continue;
            }

            if (StopWhenCustomerIsAlreadyIncluded(existing))
            {
                continue;
            }

            if (RejectCustomerNotInCustomerGroupsToAccept(customer))
            {
                continue;
            }

            if (RejectCustomerBarred(customer))
            {
                continue;
            }

            _exportData.Add(CustomerWithoutInvoiceLines(customer));
        }

        return;

        bool StopWhenCustomerIsAlreadyIncluded(List<ExportData> existing)
        {
            return existing.Count > 0;
        }

        bool RejectCustomerNotInCustomerGroupsToAccept(CustomerDto customer)
        {
            return !customerGroupsToAccept.Any(cg => cg.Equals(customer.CustomerGroupNumber));
        }

        bool RejectCustomerBarred(CustomerDto customer)
        {
            return customer.IsBarred;
        }

        ExportData CustomerWithoutInvoiceLines(CustomerDto customer)
        {
            return new ExportData
            {
                Address = customer.Address ?? string.Empty,
                CustomerNumber = customer.CustomerNumber,
                CustomerGroupNumber = customer.CustomerGroupNumber,
                Name = customer.Name ?? string.Empty,
            };
        }
    }

    // TODO change filename to more universal stream
    private async Task ExportToCsv(IEnumerable<ExportData> data, FileInfo filename, CancellationToken cancellationToken)
    {
        var headline = string.Empty
                + $"{nameof(ExportData.CustomerGroupNumber)};"
                + $"{nameof(ExportData.CustomerNumber)};"
                + $"{nameof(ExportData.Name),-50};"
                + $"{nameof(ExportData.Address),-40};"
                + $"{nameof(ExportData.InvoiceNumber)};"
                + $"{nameof(ExportData.InvoiceDate),-12} ;"
                + $"{nameof(ExportData.ProductNumber)};"
                + $"{nameof(ExportData.ProductName),-30} ;"
                + $"{nameof(ExportData.Price)} ;"
                + $"{nameof(ExportData.Quantity)} ;"
                + $"{nameof(ExportData.TotalAmount)} ;"
            ;

        await using var sw = new StreamWriter(filename.FullName, false, Encoding.UTF8);

        await sw.WriteLineAsync(headline);


        foreach (var line in data)
        {
            var bodyLine = string.Empty
                + $"{line.CustomerGroupNumber};"
                + $"{line.CustomerNumber};"
                + $"\"{line.Name}\";"
                + $"\"{line.Address.ReplaceLineEndings(", ")}\";"
                + (IsZero(line.InvoiceNumber) ? string.Empty : $"{line.InvoiceNumber,8};")
                + $"{line.InvoiceDate,12};"
                + (IsZero(line.ProductNumber) ? string.Empty : $"{line.ProductNumber,4};")
                + $"\"{line.ProductName}\";"
                + (IsZero(line.Price) ? string.Empty : $"{line.Price,10:0.000};")
                + (IsZero(line.Quantity) ? string.Empty : $"{line.Quantity,8:0.00};")
                + (IsZero(line.TotalAmount) ? string.Empty : $"{line.TotalAmount,10:0.00};")
            ;

            await sw.WriteLineAsync(bodyLine);
        }

        await sw.FlushAsync(cancellationToken);
        sw.Close();

    }

    private static bool IsZero(float value)
    {
        return Math.Abs(value) <= float.Epsilon;
    }

    private static bool IsZero(int value)
    {
        return value == 0;
    }

    public async Task<ExecutionStatus> ExportBookedInvoices(
        IInterval<DateTime> dateInterval,
        bool includeNonInvoicedCustomers,
        FileInfo outputFile,
        CancellationToken cancellationToken
    )
    {

        await LoadInvoices(dateInterval, cancellationToken);
        await MergeCustomerDetails(_invoicingApplicationSettings.CustomerGroupsToAccept, includeNonInvoicedCustomers);

        await ExportToCsv(_exportData.OrderBy(ed => ed.CustomerGroupNumber).ThenBy(ed => ed.CustomerNumber).ThenBy(ed => ed.ProductNumber), outputFile, cancellationToken);

        return new ExecutionStatus
        {
            CountFails = 0,
            Report = $"Exported {_exportData.Count}",
            Status = 0
        };
    }
}

public class ExportData
{
    public int CustomerNumber { get; set; }
    public int CustomerGroupNumber { get; set; }
    public int InvoiceNumber { get; set; }
    public string InvoiceDate { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int ProductNumber { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public float Quantity { get; set; }
    public float TotalAmount { get; set; }
    public float Price { get; set; }
}
