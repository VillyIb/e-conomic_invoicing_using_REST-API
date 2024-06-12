using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.Loader.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.Application;

public interface IInvoicingHandler
{
    Task<ExecutionStatus> LoadInvoices();
}

public class InvoicingHandler : IInvoicingHandler
{
    private readonly IEconomicGateway _economicGateway;
    private readonly ILoader _loader;
    private readonly SettingsForInvoicingApplication _settings;

    public InvoicingHandler(
        IOptions<SettingsForInvoicingApplication> settings
        // csv reader
        , IEconomicGateway economicGateway
        , ILoader loader
    )
    {
        _economicGateway = economicGateway;
        _loader = loader;
        _settings = settings.Value;
    }

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
            throw new ApplicationException($"#Invoicedate/#Bilagsdato not specified");
        }
        var invoiceDate = _loader.InvoiceDate.Value;

        if (!_loader.PaymentTerm.HasValue)
        {
            throw new ApplicationException($"#Betalingsbetingelse/#PaymentTerm not specified");
        }
        var paymetTerm = _loader.PaymentTerm.Value;

        if (_loader.CustomerGroupToAccept is null)
        {
            throw new ApplicationException($"#Kundegrupper/#CustomerGroups not specified");
        }

        var customerGroupsToAccept = _loader.CustomerGroupToAccept;

        await _economicGateway.LoadCustomerCache(customerGroupsToAccept);
        await _economicGateway.LoadProductCache();

        Console.WriteLine("");

        try
        {
            foreach (var inputInvoice in _loader.Invoices)
            {
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.Text1 = _loader.Text1!;
                inputInvoice.InvoiceDate = invoiceDate;
                inputInvoice.PaymentTerm = paymetTerm;
                await _economicGateway.PushInvoice(inputInvoice, inputInvoice.SourceFileLineNumber);
                Console.Write('.');
            }
        }
        catch (Exception ex)
        {
            // xxx
        }

        return new ExecutionStatus { Report = $"Report status {invoiceDate:yyyy-MM-dd}, {Environment.NewLine}Number of invoices: {_loader.Invoices.Count}", status = 0 };
    }
}

public class ExecutionStatus
{
    public int status { get; set; } = 0;

    public string Report { get; set; } = string.Empty;
}
