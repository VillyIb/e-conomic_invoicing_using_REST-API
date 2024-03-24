using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.Loader.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.Application;

public interface IInvoicingHandler
{
    Task<ExecutionStatus> LoadInvoices(DateTime invoiceDate);
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
    
    public async Task<ExecutionStatus> LoadInvoices(DateTime invoiceDate)
    {
        var csvFile = new FileInfo(_settings.CsvFileFullName);
        if (!csvFile.Exists)

        {
            throw new ArgumentException($"File '{csvFile.FullName}' does not exists", nameof(_settings.CsvFileFullName));
        }

        _loader.ParseCSV(csvFile);
        await _economicGateway.LoadCustomerCache(_loader.CustomerGroupToAccept);
        await _economicGateway.LoadProcuctCache();

        foreach (var inputInvoice in _loader.Invoices)
        {
            inputInvoice.InvoiceDate = invoiceDate;
            inputInvoice.Text1 = _loader.Text1;
            await  _economicGateway.PushInvoice(inputInvoice);
        }

        return new ExecutionStatus{ Report = $"Report status {invoiceDate:yyyy-MM-dd}, {Environment.NewLine}Number of invoices: {_loader.Invoices.Count}", status = 0};
    }
}

public class ExecutionStatus
{
    public int status { get; set; } = 0;

    public string Report { get; set; } = string.Empty;
}
