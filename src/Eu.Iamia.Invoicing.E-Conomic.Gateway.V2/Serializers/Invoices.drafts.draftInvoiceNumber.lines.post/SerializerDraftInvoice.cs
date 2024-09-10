using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Invoices.drafts.draftInvoiceNumber.lines.post;

public class SerializerDraftInvoice : ISerializerDraftInvoice
{
    public async Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<DraftInvoice>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new DraftInvoice();
    }
}