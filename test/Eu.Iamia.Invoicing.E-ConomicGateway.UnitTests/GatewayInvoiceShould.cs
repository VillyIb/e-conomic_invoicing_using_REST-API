using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

#region PushInvoice

public class GatewayInvoicePushInvoiceShould : GatewayBaseShould
{
    [Fact]
    public async Task PushInvoice_When_CreatedResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.Created);
        var mockedReport = Substitute.For<ICustomerReport>();

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
        var result = await sut.PushInvoice(CachedCustomerExtension.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NoContent)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.MethodNotAllowed)]
    [InlineData(HttpStatusCode.UnsupportedMediaType)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotImplemented)]
    public async Task PushInvoice_When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);
        var mockedReport = Substitute.For<ICustomerReport>();

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

        var _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.PushInvoice(CachedCustomerExtension.Valid(), new Invoice(), -9));

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }
}

#endregion

#region GetDraftInvoice

public class GatewayInvoicedReadInvoiceShould : GatewayBaseShould
{
    protected override string ResponseOK => "{\"draftInvoiceNumber\":368,\"soap\":{\"currentInvoiceHandle\":{\"id\":292}},\"templates\":{\"bookingInstructions\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates/booking-instructions\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates\"},\"attachment\":\"https://restapi.e-conomic.com/invoices/drafts/368/attachment\",\"lines\":[{\"lineNumber\":1,\"sortKey\":1,\"description\":\"Desc\",\"unit\":{\"unitNumber\":1,\"name\":\"mdr\",\"products\":\"https://restapi.e-conomic.com/units/1/products\",\"self\":\"https://restapi.e-conomic.com/units/1\"},\"product\":{\"productNumber\":\"99999\",\"self\":\"https://restapi.e-conomic.com/products/99999\"},\"quantity\":1.23,\"unitNetPrice\":1.12,\"discountPercentage\":0.00,\"unitCostPrice\":11111.00,\"totalNetAmount\":1.38,\"marginInBaseCurrency\":-13665.15,\"marginPercentage\":-990228.26}],\"date\":\"2024-04-06\",\"currency\":\"DKK\",\"exchangeRate\":100.000000,\"netAmount\":1.380000,\"netAmountInBaseCurrency\":1.38,\"grossAmount\":1.380000,\"grossAmountInBaseCurrency\":1.38,\"marginInBaseCurrency\":-13665.1500,\"marginPercentage\":-990228.26,\"vatAmount\":0.000000,\"roundingAmount\":0.00,\"costPriceInBaseCurrency\":13666.5300,\"dueDate\":\"2024-05-06\",\"paymentTerms\":{\"paymentTermsNumber\":3,\"daysOfCredit\":30,\"name\":\"30 dage\",\"paymentTermsType\":\"net\",\"self\":\"https://restapi.e-conomic.com/payment-terms/3\"},\"customer\":{\"customerNumber\":99999,\"self\":\"https://restapi.e-conomic.com/customers/99999\"},\"recipient\":{\"name\":\"Customer 1 name\",\"address\":\"Customer1 address\",\"zip\":\"3390\",\"city\":\"Customer 1 city\",\"vatZone\":{\"name\":\"Domestic\",\"vatZoneNumber\":1,\"enabledForCustomer\":true,\"enabledForSupplier\":true,\"self\":\"https://restapi.e-conomic.com/vat-zones/1\"}},\"notes\":{\"heading\":\"#99999 Customer 1 name\",\"textLine1\":\"TextLine1\"},\"layout\":{\"layoutNumber\":21,\"self\":\"https://restapi.e-conomic.com/layouts/21\"},\"pdf\":{\"download\":\"https://restapi.e-conomic.com/invoices/drafts/368/pdf\"},\"lastUpdated\":\"2024-04-06T15:49:00Z\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368\"}";

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task GetDraftInvoice_When_OkResponse_Handle_Success(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);
        var mockedReport = Substitute.For<ICustomerReport>();

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
        var result = await sut.GetDraftInvoice(HttpStatusCode.OK == statusCode ? 368 : 999);

        Mock.VerifyAll();
        mockedReport.Received(1).Info(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(368, result.DraftInvoiceNumber);
    }

    [Theory]
    [InlineData(HttpStatusCode.Created)]
    [InlineData(HttpStatusCode.NoContent)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    //[InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.MethodNotAllowed)]
    [InlineData(HttpStatusCode.UnsupportedMediaType)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotImplemented)]
    public async Task GetDraftInvoice_When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);
        var mockedReport = Substitute.For<ICustomerReport>();

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

        var _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.GetDraftInvoice(999));

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }
}

#endregion

#region Delete

public class GatewayInvoicedDeleteInvoiceShould : GatewayBaseShould
{
    protected override string ResponseOK => "{\r\n\t\"message\": \"Deleted invoice.\",\r\n\t\"deletedCount\": 1,\r\n\t\"deletedItems\": [\r\n\t\t{\r\n\t\t\t\"draftInvoiceNumber\": 403,\r\n\t\t\t\"self\": \"https://restapi.e-conomic.com/invoices/drafts/403\"\r\n\t\t}\r\n\t]\r\n}";

    [Fact]
    public async Task DeleteInvoice_When_OkResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();

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
        var result = await sut.DeleteInvoice(403);

        Mock.VerifyAll();
        mockedReport.Received(1).Info(Arg.Is<string>("DeleteInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

        Assert.True(result);
    }

    [Theory]
    [InlineData(HttpStatusCode.Created)]
    [InlineData(HttpStatusCode.NoContent)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.MethodNotAllowed)]
    [InlineData(HttpStatusCode.UnsupportedMediaType)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotImplemented)]
    public async Task DeleteDraftInvoice__When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);
        var mockedReport = Substitute.For<ICustomerReport>();

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

        var _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.DeleteInvoice(403));

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("DeleteInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }
}

#endregion
