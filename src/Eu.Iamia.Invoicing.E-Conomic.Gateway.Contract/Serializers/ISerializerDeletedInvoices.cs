using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;

public interface ISerializerDeletedInvoices
{
    DeletedInvoices Deserialize(string json);

    Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}