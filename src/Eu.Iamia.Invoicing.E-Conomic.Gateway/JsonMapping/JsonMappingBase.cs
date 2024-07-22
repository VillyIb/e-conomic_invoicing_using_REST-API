using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.JsonMapping;
public partial class JsonMappingBase
{
    private readonly RestMappingBase _restMapping;
    private readonly ISerializerDraftInvoice _serializerDraftInvoice;

    public JsonMappingBase(
        RestMappingBase restMapping 
        , ISerializerDraftInvoice serializerDraftInvoice)
    {
        _restMapping = restMapping;
        _serializerDraftInvoice = serializerDraftInvoice;
    }


}
