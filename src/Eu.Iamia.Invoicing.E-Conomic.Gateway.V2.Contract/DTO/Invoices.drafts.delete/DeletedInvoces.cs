namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.delete;

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

