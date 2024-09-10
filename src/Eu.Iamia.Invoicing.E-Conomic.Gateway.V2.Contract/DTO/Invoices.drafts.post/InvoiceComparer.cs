using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.Draft.Post;

public class InvoiceComparer : IEqualityComparer<Invoice>
{
    public bool Equals(Invoice? x, Invoice? y)
    {
        if (x == null && y == null) return true;
        if (x == null && y != null) return false;
        if (x != null && y == null) return false;

        return x == y;
    }

    public int GetHashCode(Invoice obj)
    {
        return 0;
    }
}