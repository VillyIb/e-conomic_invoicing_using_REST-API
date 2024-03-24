namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

public class CustomerCache
{
    private readonly GatewayBase _gateway;
    private readonly IList<int> _customerGroupsToAccept;

    public CustomerCache(GatewayBase gateway, IList<int> customerGroupsToAccept)
    {
        _gateway = gateway;
        _customerGroupsToAccept = customerGroupsToAccept;
        _inputCustomers = new List<CachedCustomer>();
    }

    private readonly IList<CachedCustomer> _inputCustomers;

    public CachedCustomer? GetInputCustomer(int customerNumber)
    {
        return _inputCustomers.FirstOrDefault(cus => cus.CustomerNumber == customerNumber);
    }

    public CachedCustomer Map(Collection customer)
    {
        var result = new CachedCustomer
        {
            Address = customer.address,
            City = customer.city,
            CustomerNumber = customer.customerNumber,
            Name = customer.name,
            PaymentTerms = customer.paymentTerms.paymentTermsNumber,
            Zip = customer.zip
        };

        return result;
    }

    private bool AcceptCustomer(Collection customer)
    {
        return _customerGroupsToAccept.Any(cg => cg.Equals(customer.customerGroup.customerGroupNumber));
    }

    public bool AddCustomers(CustomersHandle? customersHandle)
    {
        if (customersHandle is null) return false;

        foreach (var customer in customersHandle.collection)
        {
            var inputCustomer = Map(customer);
            if (!AcceptCustomer(customer)) continue;
            _inputCustomers.Add(inputCustomer);
        }

        return customersHandle.collection.Count() >= customersHandle.pagination.pageSize;
    }

    public async Task LoadAllCustomers()
    {
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var json = await _gateway.ReadCustomersPaged(page, 20);
            var customersHandle = CustomersHandleExtension.FromJson(json);
            @continue = AddCustomers(customersHandle) && page < 100;
            page++;
        }
    }
}

// TODO rename not "Input..."
public class CachedCustomer
{
    public int CustomerNumber { get; set; }

    public int PaymentTerms { get; set; }
    
    public string Address { get; set; }

    public string City { get; set; }

    public string Name { get; set; }

    public string Zip { get; set; }
}
