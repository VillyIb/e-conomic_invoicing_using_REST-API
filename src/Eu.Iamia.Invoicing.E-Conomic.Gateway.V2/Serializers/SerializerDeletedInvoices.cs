using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DeletedInvoices;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

[Obsolete("Tentative")]
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