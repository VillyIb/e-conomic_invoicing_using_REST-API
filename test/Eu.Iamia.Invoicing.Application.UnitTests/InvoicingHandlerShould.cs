using System.Reflection;
using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;

namespace Eu.Iamia.Invoicing.Application.UnitTests;

[NCrunch.Framework.Category("Unit")]

public class InvoicingHandlerShould
{
    private IInvoicingHandler _sut;

    public InvoicingHandlerShould()
    {
        var executingDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;

        using var setup = new Setup();
        var settingsForInvoicingApplication = setup.GetSetting<SettingsForInvoicingApplication>();
        settingsForInvoicingApplication.CsvFile = Path.Combine(executingDirectory, "TestData", "G1.csv");
        _sut = setup.GetService<IInvoicingHandler>();
    }

    [Fact]
    public async Task Test1()
    {
        var result = await _sut.LoadInvoices();
        _sut.Dispose();

        Assert.NotNull(result);
        Assert.Equal(0,result.Status);
        Assert.Equal(1,result.CountFails);
    }
}