using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayInvoiceShould : GatewayBaseShould
{
    #region PushInvoice

    [Fact]
    public async Task PushInvoice_When_OkResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PushInvoice_When_NotFound_Handle_HttpRequestException()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );

        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PushInvoice_When_NoContent_Handle_Exception()
    {
        MockResponse(HttpStatusCode.NoContent);
        var mockedReport = Substitute.For<ICustomerReport>();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );

        var result = await sut.PushInvoice(MockedCustomer.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    #endregion

    #region ReadInvoice

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(OkResponse, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadInvoice_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadInvoice();

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("ReadInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.NotEqual(OkResponse, result);
    }

    #endregion

}
