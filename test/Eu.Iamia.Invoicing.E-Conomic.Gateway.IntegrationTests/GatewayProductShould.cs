using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class GatewayProductShould
{
    private readonly IEconomicGateway _sut;

    public GatewayProductShould()
    {
        using var setup = new Setup();
        _sut = setup.GetService<IEconomicGateway>();
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task ReadProductsPaged(int page, int pageSize)
    {
        var cts = new CancellationTokenSource();
        var result = await _sut.ReadProductsPaged(page, pageSize, cts.Token);

        Assert.NotNull(result);
        Assert.NotEmpty(result.collection);
        Assert.NotNull(result.metaData);
        Assert.NotNull(result.pagination);
        Assert.NotNull(result.self);
    }
}