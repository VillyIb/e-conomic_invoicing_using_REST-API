namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

public class CustomerCache
{
    private readonly GatewayBase _gateway;

    public CustomerCache(GatewayBase gateway)
    {
        _gateway = gateway;
        _inputCustomers = new List<InputCustomer>();
    }

    private readonly IList<InputCustomer> _inputCustomers;

    public InputCustomer? GetInputCustomer(int customerNumber)
    {
        return _inputCustomers.FirstOrDefault(cus => cus.CustomerNumber == customerNumber);
    }

    public InputCustomer Map(Collection customer)
    {
        var result = new InputCustomer
        {
            Address = customer.address,
            City = customer.city,
            Country = customer.country,
            CustomerNumber = customer.customerNumber,
            Email = customer.email,
            Group = customer.customerGroup.customerGroupNumber,
            Name = customer.name,
            PaymentTerms = customer.paymentTerms.paymentTermsNumber,
            Zip = customer.zip
        };

        return result;
    }

    public bool AddCustomers(CustomersHandle? customersHandle)
    {
        if (customersHandle is null) return false;

        foreach (var customer in customersHandle.collection)
        {
            var inputCustomer = Map(customer);
            if (customer.IsClosed() || customer.IsDismissed()) continue;
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
public class InputCustomer : IInputCustomer
{
    public int CustomerNumber { get; set; }

    public int PaymentTerms { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string Zip { get; set; }

    public int Group { get; set; }
}
