using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerCustomersHandle : ISerializerCustomersHandle
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerCustomersHandle(IJsonSerializerFacade serializer)
    {
        _serializer = serializer;
    }

    public async Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<CustomersHandle>(utf8Json, cancellationToken);
    }
}