namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.DTO_Utils.Invoice;

public class InvoiceComparer : IEqualityComparer<Contract.DTO.Invoices.drafts.post.Invoice>
{
    public bool Equals(Contract.DTO.Invoices.drafts.post.Invoice? x, Contract.DTO.Invoices.drafts.post.Invoice? y)
    {
        if (x == null && y == null) return true;
        if (x == null && y != null) return false;
        if (x != null && y == null) return false;

        return x == y;
    }

    public int GetHashCode(Contract.DTO.Invoices.drafts.post.Invoice obj)
    {
        return 0;
    }
}