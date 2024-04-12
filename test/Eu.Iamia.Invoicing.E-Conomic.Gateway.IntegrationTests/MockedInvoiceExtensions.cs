using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;

public static class MockedCustomer 
{
    public static CachedCustomer Valid()
    {
        return new CachedCustomer()
        {
            CustomerNumber = 99999,
            Address = "Customer1 address",
            City = "Customer 1 city",
            Name = "Mocked Customer",
            PaymentTerms = 99999,
            Zip = "3390"
        };
    }
}

public static class MockedInvoiceExtensions
{
    public static MockedInvoice Valid(CachedCustomer customer)
    {
        const string TextLine1 = "TextLine1";

        return new MockedInvoice
        {
            Customer = new(customer.CustomerNumber),
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            Layout = new()
            {
                LayoutNumber = 21
            },
            Notes = new()
            {
                Heading = $"#{customer.CustomerNumber} {customer.Name}",
                TextLine1 = TextLine1,
                TextLine2 = null
            },
            Recipient = new()
            {
                Address = $"{customer.Address}",
                City = $"{customer.City}",
                Zip = $"{customer.Zip}",
                Name = $"{customer.Name}",
                VatZone = new()
                {
                    EnabledForCustomer = true,
                    EnabledForSupplier = true,
                    Name = "Domestic",
                    VatZoneNumber = 1
                }
            },
            References = new()
            {
                Other = null
            },
            PaymentTerms = new()
            {
                DaysOfCredit = 1,
                PaymentTermsNumber = 3,
                Name = null,
                PaymentTermsType = PaymentTermsType.net
            },
            Lines = new()
            {
                new Line
                {
                    Unit = new Unit
                    {
                        Name = "stk", UnitNumber = 1
                    },
                    Description = "Desc",
                    UnitNetPrice = 1.12,
                    Product = new Product
                    {
                        ProductNumber = "99999"
                    },
                    Quantity = 1.23,
                    DiscountPercentage = 0.0,
                    LineNumber = 1,
                    MarginInBaseCurrency = 100.0,
                    MarginPercentage = 100.0,
                    SortKey = 1,
                    TotalNetAmount = 100.0,
                    UnitCostPrice = 0.0
                }
            }
        };
    }


    public static MockedInvoice Invalid_PaymentTerm(this MockedInvoice value)
    {
        value.SetPaymentTerms(4711);
        return value;
    }

}
