using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayCustomerShould : GatewayBaseShould
{
    private static SettingsForEConomicGateway settings = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };

    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);


        using var sut = new GatewayBase(settings, HttpMessageHandler);
        var result = await sut.ReadCustomersPaged(0, 20);

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.Equal(OkResponse, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(settings, HttpMessageHandler);
        var result = await sut.ReadCustomersPaged(0, 20);

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(OkResponse, result);
    }
}
