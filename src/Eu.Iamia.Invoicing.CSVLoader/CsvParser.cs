// ReSharper disable StringLiteralTypo

using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

public class CsvParser
{
    public Metadata? Metadata => _level2Parsers.Metadata;

    private int? _sourceFileLineNumber = 0;

    private readonly ILevel2Parsers _level2Parsers;

    public IList<IInputInvoice> Invoices => _level2Parsers.Invoices;

    public CsvParser(ILevel2Parsers level2Parsers)
    {
        _level2Parsers = level2Parsers;
    }

    private int SourceFileLineNumber {
        get { return _sourceFileLineNumber ??= 0; }
        set { _sourceFileLineNumber = value;
            _level2Parsers._sourceFileLineNumber = value;
        }
    }

    public int ParseCSV(Stream st)
    {
        using var sr = new StreamReader(st);

        while (true)
        {
            var row = sr.ReadLine();
            SourceFileLineNumber++;

            if (row == null) break;
            if (!row.StartsWith("#")) continue;

            var columns = row.Split(';');

            var colA = columns[0].ToLowerInvariant();

            switch (colA)
            {
                case "#tags":
                    // parse tags row
                    _level2Parsers.ParseTagsRow(columns);
                    break;

                case "#customergroup":
                case "#customergroups":
                case "#kundegruppe":
                case "#kundegrupper":
                    // parse for customer groups to accept on customers
                    _level2Parsers.ParseCustomerGroupRow(columns);
                    break;

                case "#tekst1":
                case "#text1":
                    // parse for text1 multiple lines wil be concatenated
                    _level2Parsers.ParseText1Row(columns);
                    break;

                case "#bilagsdato":
                case "#invoicedate":
                    // parse metadata for invoice date 
                    _level2Parsers.ParseInvoiceDateRow(columns);
                    break;

                case "#betalingsbetingelse":
                case "#paymentterm":
                    // parse payment terms
                    _level2Parsers.ParsePaymentTerm(columns);
                    break;

                case "#product":
                case "#products":
                case "#produkt":
                case "#produkter":
                    // parse metadata for product number 'e-conomic varenummer' (alpha-numeric value)
                    _level2Parsers.ParseProductsRow(columns);
                    break;

                case "#tekst":
                case "#text":
                    // parse metadata for product description (text on invoice)
                    _level2Parsers.ParseInvoiceTextRow(columns);
                    break;

                case "#unittext":
                case "#enhedstekst":
                    // parse metadata for product unit text 'enheds betegnelse (m2|m3|mdr|år|kW|kWh|stk|...)' 
                    _level2Parsers.ParseUnitTextRow(columns);
                    break;

                case "#unitnumber":
                case "#enhedsnummer":
                    // parse metadata for product unit number 
                    _level2Parsers.ParseUnitNumberRow(columns);
                    break;

                case "#unitprice":
                case "#enhedspris":
                    // parse metadata for product unit price 'enheds pris'
                    _level2Parsers.ParseUnitPriceRow(columns);
                    break;

                case "#invoice":
                case "#faktura":
                    // parse for invoice and products 'Faktura og faktura linjer'
                    _level2Parsers.ParseInvoiceRow(columns);
                    break;
            }
        }

        return _level2Parsers.Invoices.Count;
    }
}