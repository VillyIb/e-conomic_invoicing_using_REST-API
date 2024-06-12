using NSubstitute;
using System.Net;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

[NCrunch.Framework.Category("Unit")]
public class GatewayProductShould : GatewayBaseShould
{
    protected override string ResponseOK => "{}";

    protected override string ResponseCreated => "{}";

    [Fact]
    public async Task ReadProductsPaged_When_OKResponse_Handle_Success()
    {
        MockResponse(HttpStatusCode.OK);
        using var sut = GetSut;

        var result = await sut.ReadProductsPaged(0, 20, Cts.Token);

        Mock.VerifyAll();
        MockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        MockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
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
    public async Task ReadProductsPaged_When_UnexpectedStatus_Handle_HttpRequestException(HttpStatusCode statusCode)
    {
        MockResponse(statusCode);
        using var sut = GetSut;

        _ = await Assert.ThrowsAsync<HttpRequestException>(() => sut.ReadProductsPaged(0, 20, Cts.Token));

        Mock.VerifyAll();
        MockedReport.Received(1).Error(Arg.Is<string>("ReadProductsPaged"), Arg.Any<string>());
        MockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());
    }


}
