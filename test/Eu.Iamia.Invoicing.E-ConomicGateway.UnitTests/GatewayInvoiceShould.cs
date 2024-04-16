using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;

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
        var mockedReport = Substitute.For<ICustomerReport>();

        using var sut = new GatewayBase(_settings, mockedReport, HttpMessageHandler);
        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_PushInvoice_Handle_NotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();

        using var sut = new GatewayBase(_settings, mockedReport, HttpMessageHandler);
        
        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(),-9);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
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
