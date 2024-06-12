using System.Net;
using NSubstitute;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayInvoicedReadInvoiceShould : GatewayBaseShould
{
    protected override string ResponseOK => "{\"draftInvoiceNumber\":368,\"soap\":{\"currentInvoiceHandle\":{\"id\":292}},\"templates\":{\"bookingInstructions\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates/booking-instructions\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates\"},\"attachment\":\"https://restapi.e-conomic.com/invoices/drafts/368/attachment\",\"lines\":[{\"lineNumber\":1,\"sortKey\":1,\"description\":\"Desc\",\"unit\":{\"unitNumber\":1,\"name\":\"mdr\",\"products\":\"https://restapi.e-conomic.com/units/1/products\",\"self\":\"https://restapi.e-conomic.com/units/1\"},\"product\":{\"productNumber\":\"99999\",\"self\":\"https://restapi.e-conomic.com/products/99999\"},\"quantity\":1.23,\"unitNetPrice\":1.12,\"discountPercentage\":0.00,\"unitCostPrice\":11111.00,\"totalNetAmount\":1.38,\"marginInBaseCurrency\":-13665.15,\"marginPercentage\":-990228.26}],\"date\":\"2024-04-06\",\"currency\":\"DKK\",\"exchangeRate\":100.000000,\"netAmount\":1.380000,\"netAmountInBaseCurrency\":1.38,\"grossAmount\":1.380000,\"grossAmountInBaseCurrency\":1.38,\"marginInBaseCurrency\":-13665.1500,\"marginPercentage\":-990228.26,\"vatAmount\":0.000000,\"roundingAmount\":0.00,\"costPriceInBaseCurrency\":13666.5300,\"dueDate\":\"2024-05-06\",\"paymentTerms\":{\"paymentTermsNumber\":3,\"daysOfCredit\":30,\"name\":\"30 dage\",\"paymentTermsType\":\"net\",\"self\":\"https://restapi.e-conomic.com/payment-terms/3\"},\"customer\":{\"customerNumber\":99999,\"self\":\"https://restapi.e-conomic.com/customers/99999\"},\"recipient\":{\"name\":\"Customer 1 name\",\"address\":\"Customer1 address\",\"zip\":\"3390\",\"city\":\"Customer 1 city\",\"vatZone\":{\"name\":\"Domestic\",\"vatZoneNumber\":1,\"enabledForCustomer\":true,\"enabledForSupplier\":true,\"self\":\"https://restapi.e-conomic.com/vat-zones/1\"}},\"notes\":{\"heading\":\"#99999 Customer 1 name\",\"textLine1\":\"TextLine1\"},\"layout\":{\"layoutNumber\":21,\"self\":\"https://restapi.e-conomic.com/layouts/21\"},\"pdf\":{\"download\":\"https://restapi.e-conomic.com/invoices/drafts/368/pdf\"},\"lastUpdated\":\"2024-04-06T15:49:00Z\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368\"}";

    [Fact]
    public async Task GetDraftInvoice_When_OkResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = GetSut;

        var result = await sut.GetDraftInvoice(368);

        Mock.VerifyAll();
        MockedReport.Received(1).Info(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        MockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(368, result.DraftInvoiceNumber);
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
    public async Task GetDraftInvoice_When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);

        using var sut = GetSut;

        var _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.GetDraftInvoice(999));

        Mock.VerifyAll();
        MockedReport.Received(1).Error(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        MockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }
}