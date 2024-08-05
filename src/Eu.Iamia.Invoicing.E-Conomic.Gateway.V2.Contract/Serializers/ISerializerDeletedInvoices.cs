using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DeletedInvoices;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerDeletedInvoices
{
    Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}