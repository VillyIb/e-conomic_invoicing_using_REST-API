using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerPaymentTermsHandle
{
    Task<PaymentTermsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}