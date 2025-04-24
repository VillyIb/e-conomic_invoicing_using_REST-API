using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.TestEnv.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiProductShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiProductShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Theory]
    [InlineData(0, 20)] // test environment contains product 1..7
    [InlineData(1, 20)]
    public async Task GetProducts(int page, int pageSize)
    {
        var stream = await _sut.GetProducts(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public async Task GetProduct(int productNumber)
    {
        var stream = await _sut.GetProduct(productNumber, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }

}