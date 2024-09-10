using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerProductsHandle : ISerializerProductsHandle
{
    public async Task<ProductsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<ProductsHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new ProductsHandle();
    }
}
