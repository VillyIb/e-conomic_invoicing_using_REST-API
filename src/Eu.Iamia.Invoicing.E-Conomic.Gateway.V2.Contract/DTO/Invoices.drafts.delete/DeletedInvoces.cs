namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.delete;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class DeletedItem
{
    public int    draftInvoiceNumber { get; set; }

    public string self               { get; set; }
}

public class DeletedInvoices
{
    public string            message      { get; set; }

    public int               deletedCount { get; set; }

    public List<DeletedItem> deletedItems { get; set; }
}

