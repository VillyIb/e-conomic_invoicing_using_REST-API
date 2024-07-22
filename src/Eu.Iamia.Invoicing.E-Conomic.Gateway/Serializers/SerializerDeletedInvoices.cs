using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

public class SerializerDeletedInvoices : ISerializerDeletedInvoices
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerDeletedInvoices(IJsonSerializerFacade serializer)
    {
        _serializer = serializer;
    }

    public DeletedInvoices Deserialize(string json)
    {
        return _serializer.Deserialize<DeletedInvoices>(json);
    }

    public async Task<DeletedInvoices> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<DeletedInvoices>(utf8Json, cancellationToken);
    }
}