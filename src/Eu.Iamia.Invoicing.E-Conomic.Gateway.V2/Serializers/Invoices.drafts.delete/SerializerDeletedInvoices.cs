using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.delete;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Invoices.drafts.delete;

[Obsolete("Tentative")]
public class SerializerDeletedInvoices : ISerializerDeletedInvoices
{
    public async Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<DeletedInvoices>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new DeletedInvoices();
    }
}