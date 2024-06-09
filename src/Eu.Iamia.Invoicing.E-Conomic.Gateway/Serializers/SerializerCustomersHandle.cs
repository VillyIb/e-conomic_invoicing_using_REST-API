using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

public class SerializerCustomersHandle : ISerializerCustomersHandle
{
    private readonly IJsonSerializerFacadeV2 _serializer;

    public SerializerCustomersHandle(IJsonSerializerFacadeV2 serializer)
    {
        _serializer = serializer;
    }

    public async Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<CustomersHandle>(utf8Json, cancellationToken);
    }
}