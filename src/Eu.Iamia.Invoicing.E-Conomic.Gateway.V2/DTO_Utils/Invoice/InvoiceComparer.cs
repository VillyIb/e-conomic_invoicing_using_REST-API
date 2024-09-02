namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.DTO_Utils.Invoice;

public class InvoiceComparer : IEqualityComparer<Contract.DTO.Invoice.Invoice>
{
    public bool Equals(Contract.DTO.Invoice.Invoice? x, Contract.DTO.Invoice.Invoice? y)
    {
        if (x == null && y == null) return true;
        if (x == null && y != null) return false;
        if (x != null && y == null) return false;

        return x == y;
    }

    public int GetHashCode(Contract.DTO.Invoice.Invoice obj)
    {
        return 0;
    }
}