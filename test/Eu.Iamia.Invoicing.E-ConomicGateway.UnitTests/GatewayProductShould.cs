using Eu.Iamia.Reporting.Contract;
using NSubstitute;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayProductShould : GatewayBaseShould
{
    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();
        var cts = new CancellationTokenSource();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDeletedInvoices(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadProductsPaged(0, 20, cts.Token);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();
        var cts = new CancellationTokenSource();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDeletedInvoices(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadProductsPaged(0, 20, cts.Token);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("ReadProductsPaged"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleNoContent()
    {
        MockResponse(HttpStatusCode.NoContent);
        var mockedReport = Substitute.For<ICustomerReport>();
        var cts = new CancellationTokenSource();

        var serializer = new JsonSerializerFacade();

        using var sut = new GatewayBaseStub(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDeletedInvoices(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadProductsPaged(0, 20, cts.Token);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("ReadProductsPaged"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }
}
