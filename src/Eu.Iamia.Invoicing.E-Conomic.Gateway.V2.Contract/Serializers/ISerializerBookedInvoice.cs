using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.bookedInvoiceNumber.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerBookedInvoice
{
    Task<BookedInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}