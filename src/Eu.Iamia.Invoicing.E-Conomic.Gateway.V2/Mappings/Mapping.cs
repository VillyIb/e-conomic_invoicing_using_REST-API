using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Mappings;

public static class Mapping
{
    public static Application.Contract.DTO.ProductDto ToProductDto(this E_Conomic.Gateway.Contract.DTO.Product.Collection restApiProduct)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var unitDto = restApiProduct.unit is null 
            ? null 
            : new Application.Contract.DTO.ProductDto.UnitDto
            {
                Name = restApiProduct.unit.name,
                UnitNumber = restApiProduct.unit.unitNumber
            }
        ;

        return new Eu.Iamia.Invoicing.Application.Contract.DTO.ProductDto
        {
            Description = restApiProduct.description,
            ProductNumber = restApiProduct.productNumber,
            Name = restApiProduct.name,
            Unit = unitDto
        };
    }

    public static Application.Contract.DTO.CustomerDto ToCustomerDto(this E_Conomic.Gateway.Contract.DTO.Customer.Collection restApiCustomer)
    {
        return new Eu.Iamia.Invoicing.Application.Contract.DTO.CustomerDto
        {
            Name = restApiCustomer.name,
            Address = restApiCustomer.address,
            City = restApiCustomer.city,
            CustomerNumber = restApiCustomer.customerNumber,
            Zip = restApiCustomer.zip,
            PaymentTerms = restApiCustomer.paymentTerms.paymentTermsNumber
        };
    }

    public static E_Conomic.Gateway.DTO.Invoice.Invoice ToRestApiInvoice(
        ProductDto inputDto, 
        E_Conomic.Gateway.DTO.Invoice.Customer restApiCustomer,
        CustomerDto customer
        
        )
    {
        var inputInvoiceInvoiceDate = DateTime.MaxValue;

        var invoice = new Invoice
        {
            Customer = restApiCustomer,
            Date = inputInvoiceInvoiceDate.ToString("yyyy-MM-dd"),
            //ExchangeRate = 100,
            //Delivery = new(
            //"delivery-address"
            //, "delivery-zip"
            //, "delivery-city"
            //, "delivery-country"
            //, DateTime.Today
            //),
            Layout = new() { LayoutNumber = _settings.LayoutNumber },
            Notes = new()
            {
                Heading = $"#{customer.CustomerNumber} {customer.Name}",
                TextLine1 = inputInvoice.Text1,
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
                    VatZoneNumber = 1 // Hardcoded value
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
                //PaymentTermsNumber = customer.PaymentTerms,
                PaymentTermsNumber = customer.PaymentTerms,
                //,
                //Name = "Lb. md. 14 dage"
                //,
                //PaymentTermsType = PaymentTermsType.invoiceMonth
            }
        };

        foreach (var inputLine in inputInvoice.InvoiceLines)
        {
            const int lineNumber = 1;

            var inputProduct = _productCache.GetInputProduct(inputLine.ProductNumber);

            if (inputProduct is null)
            {
                throw new ApplicationException($"Product: '{inputLine.ProductNumber}' not found in e-conomic, Source file line: {inputLine.SourceFileLineNumber}");
            }

            var unit = inputProduct!.Unit is null
                ? null
                : new DTO.Invoice.Unit(
                    name: inputProduct.Unit.Name,
                    unitNumber: inputProduct.Unit.UnitNumber
                );

            var line = new Line()
            {
                Description = inputLine.Description,
                LineNumber = lineNumber,
                Product = new()
                {
                    ProductNumber = inputLine.ProductNumber
                },
                Quantity = inputLine.Quantity!.Value,
                SortKey = lineNumber,
                Unit = unit,
                UnitNetPrice = inputLine.UnitNetPrice!.Value,
            };

            invoice.Lines.Add(line);
        }
    }
}
