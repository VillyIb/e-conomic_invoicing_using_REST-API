using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Reporting.Contract;
// intentional

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
internal class GatewayBaseStub : GatewayBase
{
    private readonly HttpMessageHandler _httpMessageHandler;

    internal GatewayBaseStub(
        SettingsForEConomicGateway settings,
        ISerializerCustomersHandle serializerCustomersHandle,
        ISerializerDraftInvoice serializerDraftInvoice,
        ISerializerProductsHandle serializerProductsHandle,
        ICustomerReport report, HttpMessageHandler httpMessageHandler
    ) : base(settings, serializerCustomersHandle, serializerDraftInvoice, serializerProductsHandle, report)
    {
        _httpMessageHandler = httpMessageHandler;
    }

    protected override HttpClient HttpClient => HttpClientField ??= new HttpClient(_httpMessageHandler);
}