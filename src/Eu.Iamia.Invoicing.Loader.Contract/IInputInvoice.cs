namespace Eu.Iamia.Invoicing.Loader.Contract;

public interface IInputInvoice
{
    DateTime InvoiceDate { get; set; }

    int CustomerNumber { get; set; }

    int PaymentTerm { get; set; }

    string Text1 { get; set; }

    int SourceFileLineNumber { get; set; }

    IList<IInputLine> InvoiceLines { get; set; }
}