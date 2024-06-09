using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;

public interface ISerializerDraftInvoice
{
    DraftInvoice Deserialize(string json);

    Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}