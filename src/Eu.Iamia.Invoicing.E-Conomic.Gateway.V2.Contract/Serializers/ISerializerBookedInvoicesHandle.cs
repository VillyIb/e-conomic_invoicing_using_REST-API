using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerBookedInvoicesHandle
{
    Task<BookedInvoicesHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}