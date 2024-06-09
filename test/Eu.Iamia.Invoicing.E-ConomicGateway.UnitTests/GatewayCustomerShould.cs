using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayCustomerShould : GatewayBaseShould
{
    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_Handle_OkResponse()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();
        var cts = new CancellationTokenSource();

        var serializer = new JsonSerializerFacadeV2();

        using var sut = new GatewayBase(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );
        var result = await sut.ReadCustomersPaged(0, 20, cts.Token);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();
        var cts = new CancellationTokenSource();

        var serializer = new JsonSerializerFacadeV2();

        using var sut = new GatewayBase(
            Settings,
            new SerializerCustomersHandle(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            mockedReport,
            HttpMessageHandler
        );

        var result = await sut.ReadCustomersPaged(0, 20, cts.Token);
        mockedReport.Received(1).Error(Arg.Is<string>("ReadCustomersPaged"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Mock.VerifyAll();

        Assert.NotNull(result);
    }
}
