using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

public class InputInvoice : IInputInvoice
{
    public DateTime InvoiceDate { get; set; }

    public int CustomerNumber { get; set; }

    public int PaymentTerm    { get; set; }

    public string Text1 { get; set; } = string.Empty;

    private IList<IInputLine>? _invoiceLines;

    public int SourceFileLineNumber { get; set; } = -1;

    public IList<IInputLine> InvoiceLines
    {
        get => _invoiceLines ??= new List<IInputLine>();
        set => _invoiceLines = value;
    }
}