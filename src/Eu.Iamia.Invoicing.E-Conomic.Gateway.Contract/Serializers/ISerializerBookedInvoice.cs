using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.BookedInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;

public interface ISerializerBookedInvoice
{
    Invoice Deserialize(string json);

    Task<Invoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}