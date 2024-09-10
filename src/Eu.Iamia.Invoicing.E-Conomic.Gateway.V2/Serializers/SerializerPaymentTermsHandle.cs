using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerPaymentTermsHandle : ISerializerPaymentTermsHandle
{
    public async Task<PaymentTermsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<PaymentTermsHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new PaymentTermsHandle();
    }
}
