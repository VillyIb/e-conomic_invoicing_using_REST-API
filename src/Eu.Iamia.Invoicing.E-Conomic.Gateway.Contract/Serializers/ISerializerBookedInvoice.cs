using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.BookedInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;

public interface ISerializerBookedInvoice
{
    Invoices Deserialize(string json);

    Task<Invoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}