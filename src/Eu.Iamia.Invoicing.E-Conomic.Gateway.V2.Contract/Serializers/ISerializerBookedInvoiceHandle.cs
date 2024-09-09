using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.BookedInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerBookedInvoiceHandle
{
    Task<BookedInvoicesHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}