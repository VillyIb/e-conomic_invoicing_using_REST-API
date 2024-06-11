using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;

public static class InvoiceStubExtension
{
    public static InvoiceStub Valid(CachedCustomer customer)
    {
        const string textLine1 = "TextLine1";

        return new InvoiceStub
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
                TextLine1 = textLine1,
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

    public static InvoiceStub Invalid_PaymentTerm(this InvoiceStub value)
    {
        value.SetPaymentTerms(4711);
        return value;
    }
}
