// Remote REST API data definition.

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable IDE1006
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Text.Json.Serialization;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerm;

public class PaymentTermsHandle
{
    [JsonPropertyName("collection")]
    public PaymentTerm[] PaymentTerms { get; set; }
    public Pagination pagination { get; set; }
    public string self { get; set; }
}

public class Pagination
{
    public int skipPages { get; set; }
    public int pageSize { get; set; }
    public int maxPageSizeAllowed { get; set; }
    public int results { get; set; }
    public int resultsWithoutFilter { get; set; }
    public string firstPage { get; set; }
    public string lastPage { get; set; }
}

public class PaymentTerm
{
    public int paymentTermsNumber { get; set; }
    public int daysOfCredit { get; set; }
    public string name { get; set; }
    public string paymentTermsType { get; set; }
    public string self { get; set; }
    public string description { get; set; }
}