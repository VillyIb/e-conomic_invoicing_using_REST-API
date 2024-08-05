using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DeletedInvoices;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

[Obsolete("Tentative")]
public class SerializerDeletedInvoices : ISerializerDeletedInvoices
{
    public async Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<DeletedInvoices>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new DeletedInvoices();
    }
}