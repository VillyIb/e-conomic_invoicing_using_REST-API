using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Unit = Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice.Unit;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests.Stubs;


/// <summary>
/// Used to create json to manually inject into RestApiInvoiceShould.
/// </summary>
[NCrunch.Framework.Category("Unit")]
public class InvoiceStub
{
    public static Line GetLine(int lineNumber)
    {
        var line = new Line
        {
            Description = $"Description line {lineNumber}",
            LineNumber = lineNumber,
            Product = new()
            {
                ProductNumber = "99999"
            },
            Quantity = 1,
            SortKey = lineNumber,
            Unit = new Unit("mdr", 1),
            UnitNetPrice = 1.0,
            UnitCostPrice = 0.00,
            DiscountPercentage = 0.0,
            TotalNetAmount = 1.0,
            MarginInBaseCurrency = 0.0,
            MarginPercentage = 100.00
        };

        return line;
    }

    public static Invoice GetInvoice()
    {
        var inputInvoiceCustomerNumber = 99999;
        var inputInvoiceInvoiceDate = DateTime.UtcNow;
        var layoutNumber = 21;
        var customerNumber = 99999;
        var invoiceText1 = "InvoiceText1";
        var customerAddress = "CustomerAddress";
        var customerCity = "CustomerCity";
        var customerZip = "9999";
        var customerName = "CustomerName";
        var paymentTerm = 1;

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
            Layout = new() { LayoutNumber = layoutNumber},
            Notes = new()
            {
                Heading = $"#{customerNumber} {customerName}",
                TextLine1 = invoiceText1,
                //TextLine2 = "Text2.1\nText2.2\nText2.3"
            },
            Recipient = new()
            {
                Address = $"{customerAddress}",
                City = $"{customerCity}",
                Zip = $"{customerZip}",
                Name = $"{customerName}",
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
            PaymentTerms = new PaymentTerms
            {
                //DaysOfCredit = 14
                //,
                //PaymentTermsNumber = customer.PaymentTerms,
                PaymentTermsNumber = paymentTerm,
                //,
                //Name = "Lb. md. 14 dage"
                //,
                //PaymentTermsType = PaymentTermsType.invoiceMonth
            }
        };

        invoice.Lines.Add(GetLine(1));
        invoice.Lines.Add(GetLine(2));
        invoice.Lines.Add(GetLine(3));

        return invoice;
    }

    [Fact]
    public void GetInvoiceJson()
    {
        var t1 = GetInvoice();
        var t2 = t1.ToJson();
    }
    
}
