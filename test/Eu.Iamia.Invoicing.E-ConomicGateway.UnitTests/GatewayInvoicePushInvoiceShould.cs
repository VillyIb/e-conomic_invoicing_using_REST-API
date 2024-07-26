using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Utils;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

[NCrunch.Framework.Category("Unit")]
public class GatewayInvoicePushInvoiceShould : GatewayBaseShould
{
    //[Fact]
    //public async Task PushInvoice_When_CreatedResponse_Handle_Success()
    //{
    //    MockResponse(HttpStatusCode.Created);
    //    using var sut = GetSut;

    //    var result = await sut.PushInvoice(CachedCustomerFake.Valid(), new Invoice(), -9, Cts.Token);

    //    Mock.VerifyAll();
    //    MockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
    //    MockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Any<string>());

    //    Assert.NotNull(result);
    //}

    //[Theory]
    //[InlineData(HttpStatusCode.OK)]
    //[InlineData(HttpStatusCode.NoContent)]
    //[InlineData(HttpStatusCode.BadRequest)]
    //[InlineData(HttpStatusCode.Unauthorized)]
    //[InlineData(HttpStatusCode.Forbidden)]
    //[InlineData(HttpStatusCode.NotFound)]
    //[InlineData(HttpStatusCode.MethodNotAllowed)]
    //[InlineData(HttpStatusCode.UnsupportedMediaType)]
    //[InlineData(HttpStatusCode.InternalServerError)]
    //[InlineData(HttpStatusCode.NotImplemented)]
    //public async Task PushInvoice_When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    //{
    //    MockResponse(statusCode);
    //    var mockedReport = Substitute.For<ICustomerReport>();

    //    var serializer = new JsonSerializerFacade();

    //    using var sut = new GatewayBaseStub(
    //        Settings,
    //        new SerializerCustomersHandle(serializer),
    //        new SerializerDeletedInvoices(serializer),
    //        new SerializerDraftInvoice(serializer),
    //        new SerializerProductsHandle(serializer),
    //        mockedReport,
    //        HttpMessageHandler
    //    );

    //    _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.PushInvoice(CachedCustomerFake.Valid(), new Invoice(), -9, Cts.Token));

    //    Mock.VerifyAll();
    //    mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
    //    mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    //}
}