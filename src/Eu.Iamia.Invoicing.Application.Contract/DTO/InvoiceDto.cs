namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class InvoiceDto : ValueObject<InvoiceDto>
{
    public DateTime InvoiceDate { get; set; }

    public int CustomerNumber { get; set; }

    public int PaymentTerm { get; set; }

    public string Text1 { get; set; } = string.Empty;

    private IList<InvoiceLineDto>? _invoiceLines;

    public int SourceFileLineNumber { get; set; } = -1;

    public IList<InvoiceLineDto> InvoiceLines
    {
        get => _invoiceLines ??= new List<InvoiceLineDto>();
        set => _invoiceLines = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return InvoiceDate;
        yield return CustomerNumber;
        yield return PaymentTerm;
        yield return Text1;
        yield return InvoiceLines;
        yield return SourceFileLineNumber;
        foreach (var invoiceLine in InvoiceLines)
        {
            yield return invoiceLine;
        }
    }
}