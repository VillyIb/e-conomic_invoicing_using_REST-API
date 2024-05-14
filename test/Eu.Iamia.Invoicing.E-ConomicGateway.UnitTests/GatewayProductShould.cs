using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;
using System.Net;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayProductShould : GatewayBaseShould
{
    [Fact]
    public async Task GivenMockedHandler_When_ReadProductsPaged_HandleOkResponse()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();

        using var sut = new GatewayBase(Settings, mockedReport, HttpMessageHandler);
        var result = await sut.ReadProductsPaged(0, 20);

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

        using var sut = new GatewayBase(Settings, mockedReport, HttpMessageHandler);
        var result = await sut.ReadProductsPaged(0, 20, cts.Token);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("ReadProductsPaged"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }
}
