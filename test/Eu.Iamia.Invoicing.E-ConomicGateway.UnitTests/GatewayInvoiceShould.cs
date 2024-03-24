using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayInvoiceShould : GatewayBaseShould
{
    private static SettingsForEConomicGateway _settings = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };


    [Fact]
    public async Task GivenMockedHandler_When_PushInvoice_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = new GatewayBase(_settings, HttpMessageHandler);
        var result = await sut.PushInvoice(new Invoice());

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.Equal(Response, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_PushInvoice_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(_settings, HttpMessageHandler);
        var result = await sut.PushInvoice(new Invoice());

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(Response, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = new GatewayBase(_settings, HttpMessageHandler);
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.Equal(Response, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(_settings, HttpMessageHandler);
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(Response, result);
    }

}
