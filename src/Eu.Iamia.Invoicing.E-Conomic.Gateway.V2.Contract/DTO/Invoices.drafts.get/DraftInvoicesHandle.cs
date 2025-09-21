using System.Text.Json.Serialization;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.get;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class DraftInvoicesHandle
{
    [JsonPropertyName("collection")] public DraftInvoice[] Invoices { get; set; } = [];
    public                                  Pagination      pagination { get; set; }
    public                                  string          self { get; set; }

    public static readonly DraftInvoicesHandle NullBookedInvoicesHandle = new();
}

public class Pagination
{
    public int skipPages { get; set; }
    public int pageSize { get; set; }
    public int maxPageSizeAllowed { get; set; }
    public int results { get; set; }
    public int resultsWithoutFilter { get; set; }
    public string firstPage { get; set; }
    public string previousPage { get; set; }
    public string nextPage { get; set; }
    public string lastPage { get; set; }
}

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
    public References references { get; set; }
    public Layout layout { get; set; }
    public Pdf pdf { get; set; }
    public string sent { get; set; }
    public string self { get; set; }
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
    public string address { get; set; }
    public string city { get; set; }
    public string zip { get; set; }
    public string name { get; set; }
    public VatZone vatZone { get; set; }
}


public class VatZone
{
    public bool enabledForCustomer { get; set; }
    public bool enabledForSupplier { get; set; }
    public string name { get; set; }
    public int vatZoneNumber { get; set; }
}

public class Notes
{
    public string heading { get; set; }
    public string textLine1 { get; set; }
}

public class References
{
    public string other { get; set; }
}

public class Layout
{
    public int layoutNumber { get; set; }
}

public class Pdf
{
    public string self { get; set; }
    public string url { get; set; }
    public string fileName { get; set; }
    public string contentType { get; set; }
}


