using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Utils;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

public class SerializerDraftInvoice : ISerializerDraftInvoice
{
    private readonly IJsonSerializerFacadeV2 _serializer;

    public SerializerDraftInvoice(IJsonSerializerFacadeV2 serializer)
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