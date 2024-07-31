using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;
public partial class GatewayBase
{
    [Obsolete("", true)]

    public CustomerCache? CustomerCache { get; private set; }

    [Obsolete("", true)]
    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        CustomerCache = new CustomerCache(this, customerGroupsToAccept);
        await CustomerCache.LoadAllCustomers();
    }

    public ProductCache? ProductCache { get; private set; }

    public async Task LoadProductCache()
    {
        ProductCache = new ProductCache(this);
        await ProductCache.LoadAllProducts();
    }

}
