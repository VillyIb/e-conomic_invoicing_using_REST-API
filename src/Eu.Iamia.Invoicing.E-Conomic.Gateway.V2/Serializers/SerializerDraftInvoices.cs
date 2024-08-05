using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

[Obsolete("Tentative")]
public class SerializerDraftInvoices : ISerializerDraftInvoice
{
    public async Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<DraftInvoice>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new DraftInvoice();
    }
}
