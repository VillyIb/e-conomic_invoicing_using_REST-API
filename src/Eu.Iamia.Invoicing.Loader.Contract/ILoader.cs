namespace Eu.Iamia.Invoicing.Loader.Contract;

public interface ILoader
{
    IList<IInputInvoice> Invoices { get; }

    IList<int>? CustomerGroupToAccept { get; }

    string? Text1 { get; }

    DateTime? InvoiceDate { get; }

    int? PaymentTerm { get; }

    // ReSharper disable once InconsistentNaming
    int ParseCSV(FileInfo file);
}