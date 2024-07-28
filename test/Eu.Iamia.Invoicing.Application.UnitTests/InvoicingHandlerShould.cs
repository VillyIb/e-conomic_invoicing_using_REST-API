using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.Application.UnitTests;

public class InvoicingHandlerShould
{
    private IInvoicingHandler _sut;

    public InvoicingHandlerShould()
    {
        var settingsForInvoicingApplication = new SettingsForInvoicingApplication { CsvFile = "xyz.csv" };
        IEconomicGateway economicGateway = null;
        ILoader loader = null;
        _sut = new InvoicingHandler(settingsForInvoicingApplication, economicGateway, loader);
    }

    [Fact]
    public async Task Test1()
    {
        var result = await _sut.LoadInvoices();
        Assert.NotNull(result);
    }
}