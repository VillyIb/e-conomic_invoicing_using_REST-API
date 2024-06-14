using Eu.Iamia.Utils;

using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.BookedInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

public class SerializerBookedInvoice : ISerializerBookedInvoice
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerBookedInvoice(IJsonSerializerFacade serializer)
    {
        _serializer = serializer;
    }

    public Invoice Deserialize(string json)
    {
        return _serializer.Deserialize<Invoice>(json);
    }

    public async Task<Invoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<Invoice>(utf8Json, cancellationToken);
    }
}