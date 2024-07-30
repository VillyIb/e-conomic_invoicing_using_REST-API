namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class ProductDtoCache : List<ProductDto>
{
    public ProductDto? GetProduct(string? productNumber)
    {
        return this.FirstOrDefault(prod => prod.ProductNumber.Equals(productNumber, StringComparison.OrdinalIgnoreCase));
    }
}
