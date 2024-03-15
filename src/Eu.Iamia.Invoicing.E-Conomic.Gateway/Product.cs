using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public class Product : IProduct
{
    public string ProductId { get; }

    public Product(string productId)
    {
        ProductId = productId;
    }
}
