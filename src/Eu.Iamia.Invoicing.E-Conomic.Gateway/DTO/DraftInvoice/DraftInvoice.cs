using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using System.Text.Json.Serialization;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.DraftInvoice;

public class DraftInvoice : IDraftInvoice
{
    /// <summary>
    /// A reference number for the draft invoice document. This is not the final invoice number.
    /// The final invoice number can’t be determined until the invoice is booked.
    /// </summary>
    [JsonPropertyName("draftInvoiceNumber")]
    public int DraftInvoiceNumber { get; set; }

    /// <summary>
    /// The total invoice amount in the invoice currency after all taxes and discounts have been applied.
    /// For a credit note this amount will be negative.
    /// </summary>
    [JsonPropertyName("grossAmount")]
    public double GrossAmount { get; set; }
}

//public static class DraftInvoiceExtensions
//{
//    public static DraftInvoice FromJson(string json)
//    {
//        var result = JsonSerializerFacade.Deserialize<DraftInvoice>(json);
//        return result;
//    }
//}