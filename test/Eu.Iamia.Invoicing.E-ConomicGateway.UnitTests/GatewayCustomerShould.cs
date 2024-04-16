using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Reporting.Contract;
using NSubstitute;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayCustomerShould : GatewayBaseShould
{
    private static SettingsForEConomicGateway settings = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };

    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_Handle_OkResponse()
    {
        MockResponse(HttpStatusCode.OK);
        var mockedReport = Substitute.For<ICustomerReport>();
        
        using var sut = new GatewayBase(settings, mockedReport, HttpMessageHandler);
        var result = await sut.ReadCustomersPaged(0, 20);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(OkResponse, result);
    }

    [Fact]
    public async Task GivenMockedHandler_When_ReadCustomersPaged_HandleNotFoundResponse()
    {
        MockResponse(HttpStatusCode.NotFound);
        var mockedReport = Substitute.For<ICustomerReport>();

        using var sut = new GatewayBase(settings, mockedReport, HttpMessageHandler);
        var result = await sut.ReadCustomersPaged(0, 20);
        mockedReport.Received(1).Error(Arg.Is<string>("ReadCustomersPaged"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Mock.VerifyAll();

        Assert.NotNull(result);
        Assert.NotEqual(OkResponse, result);
    }
}
