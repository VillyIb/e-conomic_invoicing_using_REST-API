using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;

public class Mapper
{
    private readonly SettingsForEConomicGateway _settings;
    private readonly CustomerCache _customerCache;
    private readonly ProductCache _productCache;

    public Mapper(SettingsForEConomicGateway settings, CustomerCache customerCache, ProductCache productCache)
    {
        _settings = settings;
        _customerCache = customerCache;
        _productCache = productCache;
    }

    private CachedCustomer CustomerMustExist(int customerNumber, int sourceFileLineNumber)
    {
        return _customerCache.GetInputCustomer(customerNumber)! 
               ?? throw new ApplicationException($"Customer does not exist: '{customerNumber}', Source file line: {sourceFileLineNumber}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputInvoice"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    /// <seealso cref="https://restdocs.e-conomic.com/#post-invoices-drafts"/>
    [Obsolete]
    public (CachedCustomer customer, Invoice ecInvoice) From(IInputInvoice inputInvoice)
    {
        var inputInvoiceCustomerNumber = inputInvoice.CustomerNumber;
        var customer = CustomerMustExist(inputInvoiceCustomerNumber, inputInvoice.SourceFileLineNumber);

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
                PaymentTermsNumber = inputInvoice.PaymentTerm,
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

        return (customer, invoice);
    }
}

