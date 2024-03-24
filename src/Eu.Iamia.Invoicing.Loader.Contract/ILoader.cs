namespace Eu.Iamia.Invoicing.Loader.Contract;

public interface ILoader
{
    IList<IInputInvoice> Invoices { get; }

    IList<int> CustomerGroupToAccept { get; }

    string Text1 { get; }

    int ParseCSV(FileInfo file);
}