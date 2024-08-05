using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

// TODO still used - move
public class SerializerDraftInvoice : ISerializerDraftInvoice
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerDraftInvoice(IJsonSerializerFacade serializer)
    {
        _serializer = serializer;
    }

    public DraftInvoice Deserialize(string json)
    {
        return _serializer.Deserialize<DraftInvoice>(json);
    }

    public async Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<DraftInvoice>(utf8Json, cancellationToken);
    }
}