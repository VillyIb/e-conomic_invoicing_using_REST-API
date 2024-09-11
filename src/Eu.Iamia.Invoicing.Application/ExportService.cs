using System.Diagnostics;
using System.Text;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Invoicing.Mapping;
using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.Application;

public interface IExportService
{
    Task<ExecutionStatus> ExportBookedInvoices(
        IInterval<DateTime> dateInterval,
        CancellationToken cancellationToken
    );
}

public class ExportService : IExportService
{
    private readonly IMappingService _mappingService;
    private readonly IEconomicGatewayV2 _economicGateway;

    private readonly List<ExportData> _exportData = new List<ExportData>();

    public ExportService(
        IMappingService mappingService,
        IEconomicGatewayV2 economicGateway
    )
    {
        _mappingService = mappingService;
        _economicGateway = economicGateway;
    }

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
            var bookedInvoicesHandle = await _economicGateway.ReadBookedInvoices(page, 20, dateInterval, cancellationToken);

            foreach (var inv in bookedInvoicesHandle.Invoices)
            {
                var bi = await _economicGateway.ReadBookedInvoice(inv.bookedInvoiceNumber, cancellationToken);
                foreach (var line in bi.lines)
                {
                    Console.Write(".");
                    var product = _mappingService.ProductDtoCache.FirstOrDefault(prod => prod.ProductNumber == line.product.productNumber)
                                  ?? new ProductDto
                                  {
                                      ProductNumber = line.product.productNumber,
                                      Name = $"Product {line.product.productNumber} not found!"
                                  };

                    if (inv.customer.customerNumber == 1132)
                    {
                        Debugger.Break();
                    }

                    var ed = new ExportData
                    {
                        Address = (inv.recipient.address ?? "").Replace("\n", ", "),
                        CustomerNumber = inv.customer.customerNumber,
                        InvoiceDate = inv.date, // TODO convert to ISO ???
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

    private async Task ExportToCsv(IEnumerable<ExportData> data, FileInfo filename, CancellationToken cancellationToken)
    {
        var headline = string.Empty
                       + $"{nameof(ExportData.CustomerNumber)};"
                       + $"{nameof(ExportData.Name)};"
                       + $"{nameof(ExportData.Address)};"
                       + $"{nameof(ExportData.InvoiceNumber)};"
                       + $"{nameof(ExportData.InvoiceDate)} ;"
                       + $"{nameof(ExportData.ProductNumber)};"
                       + $"{nameof(ExportData.ProductName)} ;"
                       + $"{nameof(ExportData.Price)} ;"
                       + $"{nameof(ExportData.Quantity)} ;"
                       + $"{nameof(ExportData.TotalAmount)} ;"
            ;

        await using var sw = new StreamWriter(filename.FullName, false, Encoding.UTF8);

        await sw.WriteLineAsync(headline);

        foreach (var line in data)
        {
            await sw.WriteAsync($"{line.CustomerNumber,5};");
            await sw.WriteAsync($"\"{line.Name,-50}\";");
            await sw.WriteAsync($"\"{line.Address,-40}\";");
            await sw.WriteAsync($"{line.InvoiceNumber,8};");
            await sw.WriteAsync($"{line.InvoiceDate};");
            await sw.WriteAsync($"{line.ProductNumber,4};");
            await sw.WriteAsync($"\"{line.ProductName,-30}\";");
            await sw.WriteAsync($"{line.Price,10:0.000};");
            await sw.WriteAsync($"{line.Quantity,8:0.00};");
            await sw.WriteAsync($"{line.TotalAmount,10:0.00};");
            await sw.WriteLineAsync();
        }

        await sw.FlushAsync(cancellationToken);
        sw.Close();
    }

    public async Task<ExecutionStatus> ExportBookedInvoices(
        IInterval<DateTime> dateInterval,
        CancellationToken cancellationToken
    )
    {
        await LoadInvoices(dateInterval, cancellationToken);

        var filename = new FileInfo($"C:\\Development\\Logfiles\\{DateTime.Now:yyyy-MM-dd_HH-mm}_BookedInvoices.csv");
        await ExportToCsv(_exportData.OrderBy(ed => ed.CustomerNumber).ThenBy(ed => ed.ProductNumber), filename, cancellationToken);

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
    public int InvoiceNumber { get; set; }
    public string InvoiceDate { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int ProductNumber { get; set; }
    public string ProductName { get; set; }
    public float Quantity { get; set; }
    public float TotalAmount { get; set; }
    public float Price { get; set; }
}
