using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.Application;

public class InvoicingHandler : IInvoicingHandler
{
    private readonly IEconomicGateway _economicGateway;
    private readonly IEconomicGatewayV2 _economicGatewayV2;
    private readonly ILoader _loader;
    private readonly ICustomerReport _customerReport;
    private readonly SettingsForInvoicingApplication _settings;

    protected CancellationTokenSource Cts = new CancellationTokenSource();
    //private IList<IInputInvoice> _loaderInvoices;

    public InvoicingHandler(
        SettingsForInvoicingApplication settings
        // csv reader
        , IEconomicGateway economicGateway
        , IEconomicGatewayV2 economicGatewayV2
        , ILoader loader
        , ICustomerReport customerReport
    )
    {
        _settings = settings;
        _economicGateway = economicGateway;
        _economicGatewayV2 = economicGatewayV2;
        _loader = loader;
        _customerReport = customerReport;
    }

    public InvoicingHandler(
        IOptions<SettingsForInvoicingApplication> settings
        // csv reader
        , IEconomicGateway economicGateway
        , IEconomicGatewayV2 economicGatewayV2
        , ILoader loader
        , ICustomerReport customerReport
    ) : this(settings.Value, economicGateway, economicGatewayV2, loader, customerReport)
    { }

    public async Task<ExecutionStatus> LoadInvoices()
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

        await _economicGateway.LoadCustomerCache(customerGroupsToAccept);
        await _economicGateway.LoadProductCache();

        Console.WriteLine("");

        var countFails = 0;

        ((GatewayV2)_economicGatewayV2).CustomerCache = ((GatewayBase)_economicGateway).CustomerCache;
        ((GatewayV2)_economicGatewayV2).ProductCache = ((GatewayBase)_economicGateway).ProductCache;
        await _economicGatewayV2.LoadProductCache();

        var loaderInvoices = _loader.Invoices ?? Array.Empty<IInputInvoice>();
        foreach (var inputInvoice in loaderInvoices)
        {
            try
            {
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.Text1 = _loader.Text1!;
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.PaymentTerm = paymentTerm;
                //await _economicGateway.PushInvoice(inputInvoice, inputInvoice.SourceFileLineNumber, Cts.Token);
                _ = await _economicGatewayV2.PushInvoice(inputInvoice, inputInvoice.SourceFileLineNumber, Cts.Token);
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
        Cts.Dispose();
    }
}