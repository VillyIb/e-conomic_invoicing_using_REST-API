using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerCustomersHandle : ISerializerCustomersHandle
{
    public async Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<CustomersHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new CustomersHandle();
    }
}