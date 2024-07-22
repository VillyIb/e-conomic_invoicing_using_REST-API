using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;
using Eu.Iamia.Utils;
using Xunit;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests.RestMapping;

[NCrunch.Framework.Category("Integration")]
public class RestMappingInvoiceShould
{
    private readonly RestMappingBase _sut;
    private readonly CancellationTokenSource _cts;
    
    public RestMappingInvoiceShould()
    {
        using var setup = new Setup();
        var settings = setup.GetSetting<SettingsForEConomicGateway>();
        var customerReport = new MockedReport();
        _cts = new CancellationTokenSource();

        _sut = new RestMappingBase(settings, customerReport);
    }

    [Fact]
    public async Task GetDraftInvoicesPaged_OK()
    {
        var stream = await _sut.GetDraftInvoices(0, 20, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync();

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Fact]
    public async Task GetBookedInvoicesPaged_OK()
    {
        var from = DateTime.Parse("2024-01-01");
        var to = DateTime.Parse("2024-01-31");
        var dateRange = Interval<DateTime>.Create(from, to);

        var stream = await _sut.GetBookedInvoices(0, 20, dateRange, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync();

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Theory]
    [InlineData(416)] // must exist as draft.
    public async Task GetDraftInvoice_OK(int invoiceNumber)
    {
        var stream = await _sut.GetDraftInvoice(invoiceNumber, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync();

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }
}
