using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Invoicing.Mapping;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.Application;

public class InvoicingHandler : IInvoicingHandler
{
    private readonly IMappingService _mappingService;
    private readonly ILoader _loader;
    private readonly ICustomerReport _customerReport;
    private readonly SettingsForInvoicingApplication _settings;

    protected CancellationTokenSource Cts = new ();

    public InvoicingHandler(
        SettingsForInvoicingApplication settings
        , IMappingService mappingService
        , ILoader loader
        , ICustomerReport customerReport
    )
    {
        _settings = settings;
        _loader = loader;
        _customerReport = customerReport;
        _mappingService = mappingService;
    }

    public InvoicingHandler(
        IOptions<SettingsForInvoicingApplication> settings
        , IMappingService mappingService
        , ILoader loader
        , ICustomerReport customerReport
    ) : this(settings.Value, mappingService, loader, customerReport)
    { }

    public async Task<ExecutionStatus> LoadInvoices(CancellationToken cancellationToken)
    {
        var csvFile = new FileInfo(_settings.CsvFile);
        if (!csvFile.Exists)
        {
            throw new ArgumentException($"File '{csvFile.FullName}' does not exists", nameof(_settings.CsvFile));
        }

        _loader.ParseCSV(csvFile);

        if (!_loader.InvoiceDate.HasValue)
        {
            throw new ApplicationException("#Invoicedate/#Bilagsdato not specified");
        }
        var invoiceDate = _loader.InvoiceDate.Value;

        if (!_loader.PaymentTerm.HasValue)
        {
            throw new ApplicationException("#Betalingsbetingelse/#PaymentTerm not specified");
        }
        var paymentTerm = _loader.PaymentTerm.Value;

        if (_loader.CustomerGroupToAccept is null)
        {
            throw new ApplicationException("#Kundegrupper/#CustomerGroups not specified");
        }

        var customerGroupsToAccept = _loader.CustomerGroupToAccept;

        await _mappingService.LoadCustomerCache(customerGroupsToAccept);
        await _mappingService.LoadProductCache();
        await _mappingService.LoadPaymentTermCache();

        Console.WriteLine("");

        var countFails = 0;

        var loaderInvoices = _loader.Invoices ?? Array.Empty<Application.Contract.DTO.InvoiceDto>();
        foreach (var inputInvoice in loaderInvoices)
        {
            try
            {
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.Text1 = _loader.Text1!;
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.PaymentTerm = paymentTerm;
                var layoutNumber = 21; // TODO get from settings
                _ = await _mappingService.PushInvoice(inputInvoice, layoutNumber, inputInvoice.SourceFileLineNumber, cancellationToken);
                Console.Write('.');
            }
            catch (Exception ex)
            {
                countFails++;
                _customerReport.Error("PushInvoice", ex.Message);
                _customerReport.Close();
            }
        }

        return new ExecutionStatus { Report = $"Report status {invoiceDate:yyyy-MM-dd}, {Environment.NewLine}Number of invoices: {loaderInvoices.Count}", Status = 0, CountFails = countFails };
    }

    public void Dispose()
    {
        _customerReport.Dispose();
    }
}