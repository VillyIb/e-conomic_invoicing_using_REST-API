using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerBookedInvoicesHandle : ISerializerBookedInvoicesHandle
{
    public async Task<BookedInvoicesHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<BookedInvoicesHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ??  BookedInvoicesHandle.NullBookedInvoicesHandle;
    }
}
