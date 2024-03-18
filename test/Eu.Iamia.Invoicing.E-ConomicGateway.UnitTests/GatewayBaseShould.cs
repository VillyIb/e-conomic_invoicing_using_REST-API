using System.Net;
using Moq;
using Moq.Protected;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayBaseShould
{
    protected readonly Mock<HttpMessageHandler> Mock = new();

    protected HttpMessageHandler HttpMessageHandler => Mock.Object;

    protected const string Response = "this is a mocked response";
    protected const string Reason = "this is a mocked reason";


    protected void MockResponse(HttpStatusCode httpStatusCode)
    {
        var mockedProtected = Mock.Protected();

        var setupApiRequest = mockedProtected.Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
        setupApiRequest.Verifiable(Times.Once);

        var apiMockedResponse =
            setupApiRequest.ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                ReasonPhrase = Reason,
                Content = new StringContent(Response)
            });

        apiMockedResponse.Verifiable();
    }
}