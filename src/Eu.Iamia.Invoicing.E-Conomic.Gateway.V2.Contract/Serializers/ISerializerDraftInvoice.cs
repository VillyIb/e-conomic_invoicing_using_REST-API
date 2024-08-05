using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DraftInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerDraftInvoice
{
    Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}