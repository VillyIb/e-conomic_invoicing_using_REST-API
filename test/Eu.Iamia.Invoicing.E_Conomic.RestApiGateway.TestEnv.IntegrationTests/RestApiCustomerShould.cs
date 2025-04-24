using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.TestEnv.IntegrationTests;

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
    [InlineData(0, 20)] // test environment only contains 5 customers no. 1..5
    [InlineData(1, 20)]
    public async Task GetCustomers(int page, int pageSize)
    {
        var stream = await _sut.GetCustomers(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }
}