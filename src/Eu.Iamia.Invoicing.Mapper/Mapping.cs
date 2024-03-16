using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eu.Iamia.Invoicing.CSVLoader;

namespace Eu.Iamia.Invoicing.Mapper;

public class Mapping
{
    private readonly CustomerCache _customerCache;

    public Mapping(CustomerCache customerCache)
    {
        _customerCache = customerCache;
    }

    public Invoice? From(InputInvoice inputInvoice)
    {
        if (!inputInvoice.CustomerNumber.HasValue)
        {
            return null;
        }

        var inputInvoiceCustomerNumber = inputInvoice.CustomerNumber!.Value;
        InputCustomer? customer = _customerCache.GetInputCustomer(inputInvoiceCustomerNumber)!;

        if (customer == null) return null;

        var inputInvoicePaymentTermsNumber = 1;
        var inputInvoiceInvoiceDate = inputInvoice.InvoiceDate!.Value;

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
                // to be taken from Customer table.
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
                PaymentTermsNumber = inputInvoicePaymentTermsNumber,
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


            var line = new Line()
            {
                LineNumber = lineNumber,
                Product = new()
                {
                    ProductNumber = inputLine.ProductNumber
                },
                Quantity = inputLineQuantity,
                UnitNetPrice = inputLineUnitNetPrice
                //, TotalNetAmount = 1068.0
                //, DiscountPercentage = 0.0
                //, UnitCostPrice = 0.0
                //, MarginInBaseCurrency = 1068.0
                //, MarginPercentage = 100.0
                ,
                SortKey = lineNumber,
                Unit = new()
                {
                    //Name = "m2" 
                    //, 
                    UnitNumber = inputLineUnitNumber
                },
                Description = inputLine.Description

            };
            invoice.Lines.Add(line);

        }

        return invoice;
    }

}

