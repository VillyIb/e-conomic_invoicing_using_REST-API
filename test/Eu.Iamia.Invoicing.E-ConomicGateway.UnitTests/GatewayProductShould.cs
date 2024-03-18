using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayProductShould : GatewayBaseShould
{

    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = new GatewayBase(HttpMessageHandler);
        var result = await sut.ReadProductsPaged(0, 20);

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.Equal(Response, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(HttpMessageHandler);
        var result = await sut.ReadProductsPaged(0, 20);

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(Response, result);
    }
}
