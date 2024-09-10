using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.bookedInvoiceNumber.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerBookedInvoice : ISerializerBookedInvoice
{
    public async Task<BookedInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<BookedInvoice>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? BookedInvoice.NullBookedInvoice;
    }
}
