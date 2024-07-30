using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Mappings;

/// <summary>
/// Mapping between Application data definition and RestApiData definition.
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Incoming RestApi-Product to ProductDto.
    /// </summary>
    /// <param name="restApiProduct"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Incoming RestApi-Customer to CustomerDto.
    /// </summary>
    /// <param name="restApiCustomer"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Outgoing CustomerDto, InvoiceDto, ProductDto to RestApi-Invoice.
    /// </summary>
    /// <param name="customerDto"></param>
    /// <param name="invoiceDto"></param>
    /// <param name="productDtoCache"></param>
    /// <param name="layoutNumber"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static E_Conomic.Gateway.DTO.Invoice.Invoice ToRestApiInvoice(
        CustomerDto customerDto,
        InvoiceDto invoiceDto,
        ProductDtoCache productDtoCache,
        int layoutNumber
        )
    {
        var inputInvoiceInvoiceDate = DateTime.MaxValue;

        var invoice = new Invoice
        {
            Customer = new E_Conomic.Gateway.DTO.Invoice.Customer (customerDto.CustomerNumber),
            Date = inputInvoiceInvoiceDate.ToString("yyyy-MM-dd"),
            //ExchangeRate = 100,
            //Delivery = new(
            //"delivery-address"
            //, "delivery-zip"
            //, "delivery-city"
            //, "delivery-country"
            //, DateTime.Today
            //),
            Layout = new() { LayoutNumber = layoutNumber },
            Notes = new()
            {
                Heading = $"#{customerDto.CustomerNumber} {customerDto.Name}",
                TextLine1 = invoiceDto.Text1
                //TextLine2 = "Text2.1\nText2.2\nText2.3"
            },
            Recipient = new()
            {
                Address = $"{customerDto.Address}",
                City = $"{customerDto.City}",
                Zip = $"{customerDto.Zip}",
                Name = $"{customerDto.Name}",
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
                //PaymentTermsNumber = customerDto.PaymentTerms,
                PaymentTermsNumber = customerDto.PaymentTerms,
                //,
                //Name = "Lb. md. 14 dage"
                //,
                //PaymentTermsType = PaymentTermsType.invoiceMonth
            }
        };

        foreach (var invoiceLineDto in invoiceDto.InvoiceLines)
        {
            const int lineNumber = 1;

            var productDto = productDtoCache.GetProduct(invoiceLineDto.ProductNumber);

            if (productDto is null)
            {
                throw new ApplicationException($"Product: '{invoiceLineDto.ProductNumber}' not found in e-conomic, Source file line: {invoiceLineDto.SourceFileLineNumber}");
            }

            var unit = productDto!.Unit is null
                ? null
                : new DTO.Invoice.Unit(
                    name: productDto.Unit.Name,
                    unitNumber: productDto.Unit.UnitNumber
                );

            var line = new Line()
            {
                Description = invoiceLineDto.Description,
                LineNumber = lineNumber,
                Product = new DTO.Invoice.Product()
                {
                    ProductNumber = invoiceLineDto.ProductNumber
                },
                Quantity = invoiceLineDto.Quantity!.Value,
                SortKey = lineNumber,
                Unit = unit,
                UnitNetPrice = invoiceLineDto.UnitNetPrice!.Value,
            };

            invoice.Lines.Add(line);
        }

        return invoice;
    }
}
