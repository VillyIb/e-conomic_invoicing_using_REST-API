using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;

public interface ISerializerProductsHandle
{
    Task<ProductsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}