using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader
{
    public interface ILevel2Parsers
    {
        int _sourceFileLineNumber { get; set; }
        IList<IInputInvoice> Invoices { get; }
        Metadata? Metadata { get; set; }

        void ParseCustomerGroupeRow(IList<string> columns);
        void ParseInvoiceDateRow(IList<string> columns);
        void ParseInvoiceRow(IList<string> columns);
        void ParseInvoiceTextRow(IList<string> columns);
        void ParsePaymentTerm(IList<string> columns);
        void ParseProductsRow(IList<string> columns);
        void ParseTagsRow(IList<string> columns);
        void ParseText1Row(IList<string> columns);
        void ParseUnitNumberRow(string[] columns);
        void ParseUnitPriceRow(IList<string> columns);
        void ParseUnitTextRow(IList<string> columns);
    }
}