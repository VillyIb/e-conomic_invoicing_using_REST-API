namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class GatewayCustomerShould
{
    [Theory]
    //[InlineData(0, 20)]
    //[InlineData(1, 20)]
    //[InlineData(2, 20)]
    //[InlineData(3, 20)]
    //[InlineData(4, 20)]
    //[InlineData(5, 20)]
    //[InlineData(6, 20)]
    //[InlineData(7, 20)]
    //[InlineData(8, 20)]
    //[InlineData(9, 20)]
    //[InlineData(10, 20)]
    //[InlineData(11, 20)]
    //[InlineData(12, 20)]
    [InlineData(13, 20)]
    //[InlineData(14, 20)]
    public async Task ReadCustomersPaged(int page, int pageSize)
    {
        var sut = new GatewayBase();
        var result = await sut.ReadCustomersPaged(page, pageSize);

        Assert.NotEmpty(result);

    }
}
