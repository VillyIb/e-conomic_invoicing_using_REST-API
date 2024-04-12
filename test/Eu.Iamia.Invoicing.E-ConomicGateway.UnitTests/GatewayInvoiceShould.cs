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
    public async Task GivenMockedHandler_When_PushInvoice_Handle_OkResponse()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = new GatewayBase(_settings, new MockedReport(), HttpMessageHandler);
        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(), -9);

        Mock.VerifyAll();

        Assert.NotNull(result);
        //Assert.Equal(OkResponse, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_PushInvoice_Handle_NotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(_settings, new MockedReport(), HttpMessageHandler);
        
        _ = await Assert.ThrowsAsync<HttpRequestException>(async () =>   await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(),-9));

        Mock.VerifyAll();

    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = new GatewayBase(_settings, new MockedReport(), HttpMessageHandler);
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.Equal(OkResponse, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);

        using var sut = new GatewayBase(_settings, new MockedReport(), HttpMessageHandler);
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(OkResponse, result);
    }

}
