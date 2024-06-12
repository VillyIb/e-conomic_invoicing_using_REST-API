using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils;
using Moq;
using Moq.Protected;
using NSubstitute;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public class GatewayBaseShould
{
    protected readonly Mock<HttpMessageHandler> Mock = new();

    protected static readonly SettingsForEConomicGateway Settings = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };

    protected HttpMessageHandler HttpMessageHandler => Mock.Object;

    protected virtual string ResponseNotFound => "{\"message\":\"Validation failed. 2 errors found.\",\"errorCode\":\"E04300\", \"developerHint\":\"Inspect validation errors and correct your request.\", \"logId\":\"86d2a1f150c392bb-CPH\", \"httpStatusCode\":400,\"errors\":{ \"paymentTerms\":{\"errors\":[{\"propertyName\":\"paymentTerms\",\"errorMessage\":\"PaymentTerms '4711' not found.\",\"errorCode\":\"E07080\",\"inputValue\":4711,\"developerHint\":\"Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .\"}]}, \"paymentTermsType\":{\"errors\":[{\"propertyName\":\"paymentTermsType\",\"errorMessage\":\"Payment terms type does not match the type on the payment terms specified.\", \"errorCode\":\"E07180\",\"inputValue\":\"invoiceMonth\",\"developerHint\":\"Either specify the matching payment terms type for the payment terms in question, or omit the property.\"}]}},\"logTime\":\"2024-03-31T21:09:13\",\"errorCount\":2}";

    protected virtual string Reason => "this is a mocked reason";

    protected virtual string ResponseCreated => "{\"draftInvoiceNumber\":368,\"soap\":{\"currentInvoiceHandle\":{\"id\":292}},\"templates\":{\"bookingInstructions\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates/booking-instructions\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368/templates\"},\"attachment\":\"https://restapi.e-conomic.com/invoices/drafts/368/attachment\",\"lines\":[{\"lineNumber\":1,\"sortKey\":1,\"description\":\"Desc\",\"unit\":{\"unitNumber\":1,\"name\":\"mdr\",\"products\":\"https://restapi.e-conomic.com/units/1/products\",\"self\":\"https://restapi.e-conomic.com/units/1\"},\"product\":{\"productNumber\":\"99999\",\"self\":\"https://restapi.e-conomic.com/products/99999\"},\"quantity\":1.23,\"unitNetPrice\":1.12,\"discountPercentage\":0.00,\"unitCostPrice\":11111.00,\"totalNetAmount\":1.38,\"marginInBaseCurrency\":-13665.15,\"marginPercentage\":-990228.26}],\"date\":\"2024-04-06\",\"currency\":\"DKK\",\"exchangeRate\":100.000000,\"netAmount\":1.380000,\"netAmountInBaseCurrency\":1.38,\"grossAmount\":1.380000,\"grossAmountInBaseCurrency\":1.38,\"marginInBaseCurrency\":-13665.1500,\"marginPercentage\":-990228.26,\"vatAmount\":0.000000,\"roundingAmount\":0.00,\"costPriceInBaseCurrency\":13666.5300,\"dueDate\":\"2024-05-06\",\"paymentTerms\":{\"paymentTermsNumber\":3,\"daysOfCredit\":30,\"name\":\"30 dage\",\"paymentTermsType\":\"net\",\"self\":\"https://restapi.e-conomic.com/payment-terms/3\"},\"customer\":{\"customerNumber\":99999,\"self\":\"https://restapi.e-conomic.com/customers/99999\"},\"recipient\":{\"name\":\"Customer 1 name\",\"address\":\"Customer1 address\",\"zip\":\"3390\",\"city\":\"Customer 1 city\",\"vatZone\":{\"name\":\"Domestic\",\"vatZoneNumber\":1,\"enabledForCustomer\":true,\"enabledForSupplier\":true,\"self\":\"https://restapi.e-conomic.com/vat-zones/1\"}},\"notes\":{\"heading\":\"#99999 Customer 1 name\",\"textLine1\":\"TextLine1\"},\"layout\":{\"layoutNumber\":21,\"self\":\"https://restapi.e-conomic.com/layouts/21\"},\"pdf\":{\"download\":\"https://restapi.e-conomic.com/invoices/drafts/368/pdf\"},\"lastUpdated\":\"2024-04-06T15:49:00Z\",\"self\":\"https://restapi.e-conomic.com/invoices/drafts/368\"}";

    // ReSharper disable once InconsistentNaming
    protected virtual string ResponseOK => "OK";

    protected CancellationTokenSource Cts = new CancellationTokenSource();

    protected ICustomerReport MockedReport = Substitute.For<ICustomerReport>();

    private readonly JsonSerializerFacade _serializer = new JsonSerializerFacade();

    internal GatewayBaseStub GetSut => new GatewayBaseStub(
        Settings,
        new SerializerCustomersHandle(_serializer),
        new SerializerDeletedInvoices(_serializer),
        new SerializerDraftInvoice(_serializer),
        new SerializerProductsHandle(_serializer),
        MockedReport,
        HttpMessageHandler
    );

    protected void MockResponse(HttpStatusCode httpStatusCode)
    {
        var mockedProtected = Mock.Protected();

        var setupApiRequest = mockedProtected.Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
        setupApiRequest.Verifiable(Times.Once);

        StringContent content;

        switch (httpStatusCode)
        {
            case HttpStatusCode.OK:
                content = new StringContent(ResponseOK);
                break;

            case HttpStatusCode.Created:
                content = new StringContent(ResponseCreated);
                break;

            case HttpStatusCode.NotFound:
                content = new StringContent(ResponseNotFound);
                break;

            case HttpStatusCode.NoContent:
                content = new StringContent(string.Empty);
                break;

            default:
                content = new StringContent(Reason);
                break;
        }

        var apiMockedResponse =
            setupApiRequest.ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                ReasonPhrase = Reason,
                Content = content
            });

        apiMockedResponse.Verifiable();
    }
}