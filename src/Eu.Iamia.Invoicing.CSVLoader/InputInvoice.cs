using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

public class InputInvoice : IInputInvoice
{
    public DateTime InvoiceDate { get; set; }

    public int    CustomerNumber { get; set; }

    public string Text1 { get; set; } = string.Empty;

    private IList<IInputLine>? _invoiceLines;

    public IList<IInputLine> InvoiceLines
    {
        get => _invoiceLines ??= new List<IInputLine>();
        set => _invoiceLines = value;
    }
}