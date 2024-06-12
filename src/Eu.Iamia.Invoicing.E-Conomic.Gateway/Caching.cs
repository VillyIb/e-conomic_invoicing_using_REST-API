using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;
public partial class GatewayBase
{
    private CustomerCache? CustomerCache { get; set; }

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        CustomerCache = new CustomerCache(this, customerGroupsToAccept);
        await CustomerCache.LoadAllCustomers();
    }

    private ProductCache? ProductCache { get; set; }

    public async Task LoadProductCache()
    {
        ProductCache = new ProductCache(this);
        await ProductCache.LoadAllProducts();
    }

}
