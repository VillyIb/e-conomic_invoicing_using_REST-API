﻿using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Reporting.Contract;

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

public class CachedCustomer : ICachedCustomer, ICustomer
{
    public int CustomerNumber { get; init; }

    public int PaymentTerms { get; init; }
    
    public string? Address { get; init; }

    public string? City { get; init; }

    public string? Name { get; init; }

    public string? Zip { get; init; }
}
