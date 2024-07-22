using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
public class SerializerDraftInvoices
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerDraftInvoices(IJsonSerializerFacade serializer)
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
