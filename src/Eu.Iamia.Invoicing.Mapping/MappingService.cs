using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

namespace Eu.Iamia.Invoicing.Mapping;

public interface IMappingService
{
    Task LoadCustomerCache(IList<int> customerGroupsToAccept);
    Task LoadProductCache();
}

public  class MappingService : IMappingService
{
    private readonly IEconomicGatewayV2 _economicGateway;

    public MappingService(IEconomicGatewayV2 economicGateway)
    {
        _economicGateway = economicGateway;
    }

    private IList<CustomerDto> CustomersCache = new List<CustomerDto>();

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        CustomersCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var customersHandle = await _economicGateway.ReadCustomersPaged(page, 20, cts.Token);
            foreach (var collection in customersHandle.collection)
            {
                if (!customerGroupsToAccept.Any(cg => cg.Equals(collection.customerGroup.customerGroupNumber))) continue;

                var customerDto = collection.ToCustomerDto();
                CustomersCache.Add(customerDto);
            }
            @continue = customersHandle.collection.Any() && page < 100;
            page++;
        }
    }

    private IList<ProductDto> ProductsCache = new List<ProductDto>();

    public async Task LoadProductCache()
    {
        ProductsCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var productsHandle = await _economicGateway.ReadProductsPaged(page, 20, cts.Token);
            foreach (var collection in productsHandle.collection)
            {
                var productDto = collection.ToProductDto();
                ProductsCache.Add(productDto);
            }
            @continue = productsHandle.collection.Any() && page < 100;
            page++;
        }
    }




}
