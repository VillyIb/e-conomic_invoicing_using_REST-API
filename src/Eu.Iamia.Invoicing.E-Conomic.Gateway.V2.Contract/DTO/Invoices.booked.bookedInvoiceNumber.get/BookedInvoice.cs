namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.bookedInvoiceNumber.get;

// see: https://restapi.e-conomic.com/schema/invoices.booked.bookedInvoiceNumber.get.schema.json

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class BookedInvoice
{
    public int          bookedInvoiceNumber       { get; set; }
    public int          orderNumber               { get; set; }
    public string       date                      { get; set; }
    public string       currency                  { get; set; }
    public float        exchangeRate              { get; set; }
    public float        netAmount                 { get; set; }
    public float        netAmountInBaseCurrency   { get; set; }
    public float        grossAmount               { get; set; }
    public float        grossAmountInBaseCurrency { get; set; }
    public float        vatAmount                 { get; set; }
    public float        roundingAmount            { get; set; }
    public float        remainder                 { get; set; }
    public float        remainderInBaseCurrency   { get; set; }
    public string       dueDate                   { get; set; }
    public Paymentterms paymentTerms              { get; set; }
    public Customer     customer                  { get; set; }
    public Recipient    recipient                 { get; set; }
    public Notes        notes                     { get; set; }
    public Layout       layout                    { get; set; }
    public Pdf          pdf                       { get; set; }
    public Line[]       lines                     { get; set; }
    public string       sent                      { get; set; }
    public string       self                      { get; set; }

    public static readonly BookedInvoice NullBookedInvoice = new();
}

public class Paymentterms
{
    public int    paymentTermsNumber { get; set; }
    public int    daysOfCredit       { get; set; }
    public string name               { get; set; }
    public string paymentTermsType   { get; set; }
    public string self               { get; set; }
}

public class Customer
{
    public int    customerNumber { get; set; }
    public string self           { get; set; }
}

public class Recipient
{
    public string  name    { get; set; }
    public string  address { get; set; }
    public string  zip     { get; set; }
    public string  city    { get; set; }
    public string  country { get; set; }
    public Vatzone vatZone { get; set; }
}

public class Vatzone
{
    public string name               { get; set; }
    public int    vatZoneNumber      { get; set; }
    public bool   enabledForCustomer { get; set; }
    public bool   enabledForSupplier { get; set; }
    public string self               { get; set; }
}

public class Notes
{
    public string heading { get; set; }
}

public class Layout
{
    public int    layoutNumber { get; set; }
    public string self         { get; set; }
}

public class Pdf
{
    public string download { get; set; }
}

public class Line
{
    public int     lineNumber         { get; set; }
    public int     sortKey            { get; set; }
    public string  description        { get; set; }
    public float   quantity           { get; set; }
    public float   unitNetPrice       { get; set; }
    public float   discountPercentage { get; set; }
    public float   unitCostPrice      { get; set; }
    public float   vatRate            { get; set; }
    public float   vatAmount          { get; set; }
    public float   totalNetAmount     { get; set; }
    public Product product            { get; set; }
}

public class Product
{
    public string productNumber { get; set; }
    public string self          { get; set; }
}