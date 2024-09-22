using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiCustomerShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiCustomerShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Theory]
    //[InlineData(0, 20)]
    //[InlineData(1, 20)]
    //[InlineData(2, 20)]
    //[InlineData(3, 20)]
    //[InlineData(4, 20)]
    //[InlineData(5, 20)]
    //[InlineData(6, 20)]
    //[InlineData(7, 20)]
    [InlineData(12, 20)]
    //[InlineData(13, 20)]
    public async Task GetCustomers(int page, int pageSize)
    {
        var stream = await _sut.GetCustomers(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }
}