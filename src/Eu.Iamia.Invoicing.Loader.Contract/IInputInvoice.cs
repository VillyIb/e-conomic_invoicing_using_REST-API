namespace Eu.Iamia.Invoicing.Loader.Contract;

public interface IInputInvoice
{
    DateTime InvoiceDate { get; set; }

    int CustomerNumber { get; set; }

    string Text1 { get; set; }

    IList<IInputLine> InvoiceLines { get; set; }
}