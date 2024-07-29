namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class InvoiceDto : IInvoiceDto
{
    public DateTime InvoiceDate { get; set; }

    public int CustomerNumber { get; set; }

    public int PaymentTerm { get; set; }

    public string Text1 { get; set; } = string.Empty;

    private IList<IInvoiceLineDto>? _invoiceLines;

    public int SourceFileLineNumber { get; set; } = -1;

    public IList<IInvoiceLineDto> InvoiceLines
    {
        get => _invoiceLines ??= new List<IInvoiceLineDto>();
        set => _invoiceLines = value;
    }
}

public interface IInvoiceDto
{
    DateTime InvoiceDate { get; set; }

    int CustomerNumber { get; set; }

    int PaymentTerm { get; set; }

    string Text1 { get; set; }

    int SourceFileLineNumber { get; set; }

    IList<IInvoiceLineDto> InvoiceLines { get; set; }
}

public class InvoiceLineDto : IInvoiceLineDto
{
    public string? UnitText { get; set; }

    public int UnitNumber { get; set; }

    public string? ProductNumber { get; set; }

    public double? Quantity { get; set; }

    public double? UnitNetPrice { get; set; }

    public string? Description { get; set; }

    public int? SourceFileLineNumber { get; set; }

    public int SourceFileLine { get; set; }
}

public interface IInvoiceLineDto
{
    string? UnitText { get; set; }

    string? ProductNumber { get; set; }

    int UnitNumber { get; set; }

    double? Quantity { get; set; }

    double? UnitNetPrice { get; set; }

    string? Description { get; set; }

    int? SourceFileLineNumber { get; set; }
}
