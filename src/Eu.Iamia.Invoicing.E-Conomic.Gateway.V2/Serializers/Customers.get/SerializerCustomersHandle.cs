using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customers.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Customers.get;

public class SerializerCustomersHandle : ISerializerCustomersHandle
{
    public async Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<CustomersHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new CustomersHandle();
    }
}