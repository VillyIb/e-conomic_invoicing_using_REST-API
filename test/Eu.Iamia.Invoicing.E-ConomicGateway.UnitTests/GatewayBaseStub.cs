using Eu.Iamia.Invoicing.E_Conomic.Gateway;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;
internal class GatewayBaseStub : GatewayBase
{
    private readonly HttpMessageHandler _httpMessageHandler;

    internal GatewayBaseStub(
        SettingsForEConomicGateway settings,
        ISerializerCustomersHandle serializerCustomersHandle,
        ISerializerDeletedInvoices serializerDeletedInvoices,
        ISerializerDraftInvoice serializerDraftInvoice,
        ISerializerProductsHandle serializerProductsHandle,
        ICustomerReport report, HttpMessageHandler httpMessageHandler
    ) : base(settings, serializerCustomersHandle, serializerDeletedInvoices, serializerDraftInvoice, serializerProductsHandle, report)
    {
        _httpMessageHandler = httpMessageHandler;
    }

    protected override HttpClient HttpClient => HttpClientField ??= new HttpClient(_httpMessageHandler);
}