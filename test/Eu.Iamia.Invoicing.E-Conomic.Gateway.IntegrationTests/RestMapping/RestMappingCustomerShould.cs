using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;
using Xunit;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests.RestMapping;

[NCrunch.Framework.Category("Integration")]
public class RestMappingCustomerShould
{
    private readonly RestMappingBase _sut;
    private readonly CancellationTokenSource _cts;

    public RestMappingCustomerShould()
    {
        using var setup = new Setup();

        var settings = setup.GetSetting<SettingsForEConomicGateway>();
        var customerReport = new MockedReport();
        _cts = new CancellationTokenSource();

        _sut = new RestMappingBase(settings, customerReport);
    }

    [Fact]
    public async Task GetCustomersPaged_OK()
    {
        var stream = await _sut.GetCustomersPaged(0, 20, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync();

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }
}
