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
    public async Task PushInvoice_When_OkResponse_Handle_Success()
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
        var result = await sut.PushInvoice(CachedCustomerExtension.Valid(), new Invoice(), -9);

        Mock.VerifyAll();
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());
        mockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PushInvoice_When_NotFound_Handle_HttpRequestException()
    {
        MockResponse(HttpStatusCode.NotFound);
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
        mockedReport.Received(1).Error(Arg.Is<string>("PushInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task PushInvoice_When_NoContent_Handle_Fail()
    {
        MockResponse(HttpStatusCode.NoContent);
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
        mockedReport.Received(1).Info(Arg.Is<string>("PushInvoice"), Arg.Is<string>($"Response: {HttpStatusCode.NoContent}"));

        Assert.NotNull(result);
    }
}

#endregion

#region ReadInvoice

public class GatewayInvoicedReadInvoiceShould : GatewayBaseShould
{
    [Fact]
    public async Task GetDraftInvoice_When_OkResponse_Handle_Success()
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
        var result = await sut.GetDraftInvoice(368);

        Mock.VerifyAll();
        mockedReport.Received(1).Info(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(368, result.DraftInvoiceNumber);
    }

    [Fact]
    public async Task GetDraftInvoice_When_NotFoundResponse_Handle_HttpRequestException()
    {
        MockResponse(HttpStatusCode.NotFound);
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
        var result = await sut.GetDraftInvoice(999);

        Mock.VerifyAll();
        mockedReport.Received(1).Error(Arg.Is<string>("GetDraftInvoice"), Arg.Any<string>());
        mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(-1, result.DraftInvoiceNumber);
    }

    [Fact]
    public async Task GetDraftInvoice_When_NoContentResponse_Handle_Fail()
    {
        MockResponse(HttpStatusCode.NoContent);
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
        var result = await sut.GetDraftInvoice(999);

        Mock.VerifyAll();
        mockedReport.Received(1).Info(Arg.Is<string>("GetDraftInvoice"),
            Arg.Is<string>($"Response: {HttpStatusCode.NoContent}"));
        mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

        Assert.NotNull(result);
        Assert.Equal(-1, result.DraftInvoiceNumber);
    }

    #endregion

    #region Delete

    public class GatewayInvoicedDeleteInvoiceShould : GatewayBaseShould
    {
        protected override string OkResponse => "{\r\n\t\"message\": \"Deleted invoice.\",\r\n\t\"deletedCount\": 1,\r\n\t\"deletedItems\": [\r\n\t\t{\r\n\t\t\t\"draftInvoiceNumber\": 403,\r\n\t\t\t\"self\": \"https://restapi.e-conomic.com/invoices/drafts/403\"\r\n\t\t}\r\n\t]\r\n}";

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

        [Fact]
        public async Task DeleteDraftInvoice_When_NotFoundResponse_Handle_HttpRequestException()
        {
            MockResponse(HttpStatusCode.NotFound);
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
            var result = await sut.DeleteInvoice(999);

            Mock.VerifyAll();
            mockedReport.Received(1).Error(Arg.Is<string>("DeleteInvoice"), Arg.Any<string>());
            mockedReport.Received(0).Info(Arg.Any<string>(), Arg.Any<string>());

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteDraftInvoice_When_NoContentResponse_Handle_Fail()
        {
            MockResponse(HttpStatusCode.NoContent);
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
            var result = await sut.DeleteInvoice(999);

            Mock.VerifyAll();
            mockedReport.Received(1).Info(Arg.Is<string>("DeleteInvoice"),
                Arg.Is<string>($"Response: {HttpStatusCode.NoContent}"));
            mockedReport.Received(0).Error(Arg.Any<string>(), Arg.Any<string>());

            Assert.False(result);
        }
    }
}

#endregion
