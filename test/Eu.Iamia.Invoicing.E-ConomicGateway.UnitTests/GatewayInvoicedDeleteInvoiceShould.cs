using System.Net;
using NSubstitute;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

[NCrunch.Framework.Category("Unit")]
public class GatewayInvoicedDeleteInvoiceShould : GatewayBaseShould
{
    protected override string ResponseOK => "{\r\n\t\"message\": \"Deleted invoice.\",\r\n\t\"deletedCount\": 1,\r\n\t\"deletedItems\": [\r\n\t\t{\r\n\t\t\t\"draftInvoiceNumber\": 403,\r\n\t\t\t\"self\": \"https://restapi.e-conomic.com/invoices/drafts/403\"\r\n\t\t}\r\n\t]\r\n}";

    [Fact]
    public async Task DeleteDraftInvoice_When_OkResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.OK);

        using var sut = GetSut;

        var result = await sut.DeleteDraftInvoice(403);

        Mock.VerifyAll();
        MockedReport.Received(1).Info(Arg.Is<string>("DeleteDraftInvoice"), Arg.Any<string>());
        MockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

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

        using var sut = GetSut;

        var _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.DeleteDraftInvoice(403));

        Mock.VerifyAll();
        MockedReport.Received(1).Error(Arg.Is<string>("DeleteDraftInvoice"), Arg.Any<string>());
        MockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }
}