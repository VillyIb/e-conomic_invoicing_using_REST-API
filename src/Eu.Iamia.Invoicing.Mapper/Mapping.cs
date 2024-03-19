using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.Mapper;

public class Mapping
{
    private readonly CustomerCache _customerCache;
    private readonly ProductCache _productCache;

    public Mapping(CustomerCache customerCache, ProductCache productCache)
    {
        _customerCache = customerCache;
        _productCache = productCache;
    }

    public Invoice? From(IInputInvoice inputInvoice)
    {
        var inputInvoiceCustomerNumber = inputInvoice.CustomerNumber;
        InputCustomer? customer = _customerCache.GetInputCustomer(inputInvoiceCustomerNumber)!;

        if (customer == null) return null;

        var inputInvoiceInvoiceDate = inputInvoice.InvoiceDate;

        var invoice = new Invoice
        {
            Customer = new(inputInvoiceCustomerNumber),
            Date = inputInvoiceInvoiceDate.ToString("yyyy-MM-dd"),
            //ExchangeRate = 100,
            //Delivery = new(
            //"delivery-address"
            //, "delivery-zip"
            //, "delivery-city"
            //, "delivery-country"
            //, DateTime.Today
            //),
            Layout = new() { LayoutNumber = 21 },
            Notes = new()
            {
                Heading = $"#{customer.CustomerNumber} {customer.Name}",
                TextLine1 = "Årlig ØD fakturering \n2024",
                //TextLine2 = "Text2.1\nText2.2\nText2.3"
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
                //Other = "references-other"
            },
            PaymentTerms = new()
            {
                //DaysOfCredit = 14
                //,
                PaymentTermsNumber = customer.PaymentTerms,
                //,
                //Name = "Lb. md. 14 dage"
                //,
                //PaymentTermsType = PaymentTermsType.invoiceMonth
            }
        };

        foreach (var inputLine in inputInvoice.InvoiceLines)
        {
            var lineNumber = 1;

            var inputLineQuantity = inputLine.Quantity!.Value;
            var inputLineUnitNetPrice = inputLine.UnitNetPrice!.Value;
            var inputLineUnitNumber = inputLine.UnitNumber;
            var _ = inputLine.UnitText;

            var inputProduct = _productCache.GetInputProduct(inputLine.ProductNumber);

            if (inputProduct is null)
            {
                throw new ApplicationException($"Product: '{inputLine.ProductNumber}' not found in e-conomic.");
            }

            E_Conomic.Gateway.DTO.Invoice.Unit? unit = inputProduct!.Unit is null 
                ? null 
                : new E_Conomic.Gateway.DTO.Invoice.Unit
                {
                    Name = inputProduct.Unit.Name,
                    UnitNumber = inputProduct.Unit.UnitNumber
                };
            

            var line = new Line()
            {
                Description = inputLine.Description,
                LineNumber = lineNumber,
                Product = new()
                {
                    ProductNumber = inputLine.ProductNumber
                },
                Quantity = inputLineQuantity,
                SortKey = lineNumber,
                Unit = unit,
                UnitNetPrice = inputLineUnitNetPrice,
            };

            invoice.Lines.Add(line);
        }

        return invoice;
    }

}

