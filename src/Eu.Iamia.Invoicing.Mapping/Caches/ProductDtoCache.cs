using Eu.Iamia.Invoicing.Application.Contract.DTO;

namespace Eu.Iamia.Invoicing.Mapping.Caches;

public class ProductDtoCache : List<ProductDto>
{
    public ProductDto? GetProduct(string? productNumber)
    {
        return this.FirstOrDefault(prod => prod.ProductNumber.Equals(productNumber, StringComparison.OrdinalIgnoreCase));
    }
}