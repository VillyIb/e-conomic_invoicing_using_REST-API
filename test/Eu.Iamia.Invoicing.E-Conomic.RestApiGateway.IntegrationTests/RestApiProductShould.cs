using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.IntegrationTests;

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
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetProductsPaged(int page, int pageSize)
    {
        var stream = await _sut.GetProductsPaged(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Theory]
    [InlineData(99999)]
    public async Task GetProduct(int productNumber)
    {
        var stream = await _sut.GetProduct(productNumber, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

}