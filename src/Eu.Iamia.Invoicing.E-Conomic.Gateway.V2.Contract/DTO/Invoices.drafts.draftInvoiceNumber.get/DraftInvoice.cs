namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.get;

// see: https://restapi.e-conomic.com/schema/invoices.drafts.draftInvoiceNumber.get.schema.json

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class DraftInvoice
{
    public int draftInvoiceNumber { get; set; }
    public int orderNumber { get; set; }
    public string date { get; set; }
    public string currency { get; set; }
    public float exchangeRate { get; set; }
    public float netAmount { get; set; }
    public float netAmountInBaseCurrency { get; set; }
    public float grossAmount { get; set; }
    public float grossAmountInBaseCurrency { get; set; }
    public float vatAmount { get; set; }
    public float roundingAmount { get; set; }
    public float remainder { get; set; }
    public float remainderInBaseCurrency { get; set; }
    public string dueDate { get; set; }
    public Paymentterms paymentTerms { get; set; }
    public Customer customer { get; set; }
    public Recipient recipient { get; set; }
    public Notes notes { get; set; }
    public Layout layout { get; set; }
    public Pdf pdf { get; set; }
    public string sent { get; set; }
    public string self { get; set; }

    public static readonly DraftInvoice NullDraftInvoice = new();
}

public class Paymentterms
{
    public int paymentTermsNumber { get; set; }
    public int daysOfCredit { get; set; }
    public string name { get; set; }
}

public class Customer
{
    public int customerNumber { get; set; }
    public string name { get; set; }
    public string self { get; set; }
}

public class Recipient
{
    public string name { get; set; }
    public string address { get; set; }
    public string postalCode { get; set; }
    public string city { get; set; }
    public string country { get; set; }
}

public class Notes
{
    public string text { get; set; }
}

public class Layout
{
    public int layoutNumber { get; set; }
    public string name { get; set; }
}

public class Pdf
{
    public string self { get; set; }
    public string download { get; set; }
}

public class References
{
    public string other { get; set; }
}
