using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DeletedInvoices;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerDeletedInvoices
{
    DeletedInvoices Deserialize(string json);

    Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}