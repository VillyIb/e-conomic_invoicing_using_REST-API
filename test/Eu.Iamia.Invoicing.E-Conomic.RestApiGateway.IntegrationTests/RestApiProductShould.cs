using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiProductShould
{
    private readonly IRestApiGateway _sut;

    public RestApiProductShould()
    {
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetCustomersPaged(int page, int pageSize)
    {
        var cts = new CancellationTokenSource();
        var stream = await _sut.GetCustomersPaged(page, pageSize, cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }
}