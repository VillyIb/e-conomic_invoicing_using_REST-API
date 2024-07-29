using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

public class ProductCache
{
    private readonly GatewayBase _gateway;

    public ProductCache(GatewayBase gateway)
    {
        _gateway = gateway;
        _inputProducts = new List<InputProduct>();
    }

    private readonly IList<InputProduct> _inputProducts;

    public InputProduct? GetInputProduct(string? productNumber)
    {
        return _inputProducts.FirstOrDefault(cus => cus.ProductNumber.Equals(productNumber, StringComparison.InvariantCultureIgnoreCase));
    }

    public InputProduct Map(Collection product)
    {
        var result = new InputProduct
        {
            Description = product.description,
            Name = product.name,
            ProductNumber = product.productNumber,
        };

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (product.unit is not null)
        {
            result.Unit = new InputUnit
            {
                Name = product.unit.name,
                UnitNumber = product.unit.unitNumber
            };
        }

        return result;
    }

    public bool AddProducts(ProductsHandle? productsHandle)
    {
        if (productsHandle is null) return false;

        foreach (var product in productsHandle.collection)
        {
            var inputProduct = Map(product);
            _inputProducts.Add(inputProduct);
        }

        return productsHandle.collection.Count() >= productsHandle.pagination.pageSize;
    }

    // TODO Does NOT belong to a simple class. 
    public async Task LoadAllProducts()
    {
        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var productsHandle = await _gateway.ReadProductsPaged(page, 20, cts.Token);
            @continue = AddProducts(productsHandle) && page < 100;
            page++;
        }
    }
}

/// <summary>
/// Local representation of product.
/// </summary>
public class InputProduct
{
    public string Description { get; set; } = string.Empty;

    public InputUnit? Unit { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ProductNumber { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{nameof(ProductNumber)}: {ProductNumber}, {nameof(Name)}: {Name} ";
    }
}

public class InputUnit
{
    public string Name { get; set; } = string.Empty;

    public int UnitNumber { get; set; }
}

