using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customer;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerCustomersHandle
{
    Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}