using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

/// <summary>
/// Loads invoices from .csv file.
/// One line is One invoice with multiple lines.
/// </summary>
public class Loader
{
    private readonly FileInfo _file;

    public Loader(FileInfo file)
    {
        _file = file;
    }

    private List<IInputInvoice>? _invoices;

    private Metadata Metadata { get; set; }

    public IList<IInputInvoice> Invoices => _invoices ??= new List<IInputInvoice>();

    internal void ParseTagsRow(IList<string> columns)
    {
        Metadata = new Metadata();

        var columnIndex = 1;
        foreach (var column in columns.Skip(1))
        {

            switch (column.ToLowerInvariant())
            {
                case "#customernumber":
                    Metadata.CustomerNumberColumn = columnIndex;
                    break;
                case "#product":
                    {
                        Metadata.ProductMetadata.Add(new ProductMetadata
                        {
                            Column = columnIndex
                        });
                        break;
                    }
            }
            columnIndex++;
        }
    }

    internal void ParseProductsRow(IList<string> columns)
    {
        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            productMetadata.ProductId = columns[productMetadata.Column];
        }
    }

    internal void ParseTextRow(IList<string> columns)
    {
        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            productMetadata.InvoiceLineText = columns[productMetadata.Column];
        }
    }

    internal void ParseUnitTextRow(IList<string> columns)
    {
        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            productMetadata.UnitText = columns[productMetadata.Column];
        }
    }

    internal void ParseUnitPriceRow(IList<string> columns)
    {
        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            productMetadata.UnitPrice = double.Parse(columns[productMetadata.Column]);
        }
    }

    internal void ParseUnitNumberRow(string[] columns)
    {
        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            productMetadata.UnitNumber = (int)double.Parse(columns[productMetadata.Column]);
        }
    }


    internal void ParseInvoiceRow(IList<string> columns)
    {
        if (!int.TryParse(columns[Metadata.CustomerNumberColumn], out var customerNumber) || customerNumber <= 0) return;

        var invoice = new InputInvoice
        {
            CustomerNumber = customerNumber
        };

        foreach (var productMetadata in Metadata.ProductMetadata)
        {
            if (!double.TryParse(columns[productMetadata.Column], out var quantity) || quantity <= 0) continue;

            var inputLine = new InputLine
            {
                UnitNetPrice = productMetadata.UnitPrice,
                Description = productMetadata.InvoiceLineText,
                ProductNumber = productMetadata.ProductId,
                UnitText = productMetadata.UnitText,
                UnitNumber = productMetadata.UnitNumber,
                Quantity = quantity
            };

            invoice.InvoiceLines.Add(inputLine);
        }

        Invoices.Add(invoice);
    }


    public int ParseInvoiceFile()
    {
        using var fs = _file.OpenRead();
        using var sr = new System.IO.StreamReader(fs);

        while (true)
        {
            var row = sr.ReadLine();
            if (row == null) break;
            if (!row.StartsWith("#")) continue;

            var columns = row.Split(';');

            var colA = columns[0].ToLowerInvariant();

            switch (colA)
            {
                case "#tags":
                    // parse tags row
                    ParseTagsRow(columns);
                    break;
                case "#product":
                case "#products":
                    // parse metadata for product number 'e-conomic varenummer' 
                    ParseProductsRow(columns);
                    break;
                case "#text":
                    // parse metadata for product description 'varelinje text (text on invoice)
                    ParseTextRow(columns);
                    break;
                case "#unittext":
                    // parse metadata for product unit text 'enheds betegnelse (m2|m3|mdr|år|kW|kWh|stk|...)' 
                    ParseUnitTextRow(columns);
                    break;
                case "#unitnumber":
                    // parse metadata for product unit number 
                    ParseUnitNumberRow(columns);
                    break;
                case "#unitprice":
                    // parse metadata for product unit price 'enheds pris'
                    ParseUnitPriceRow(columns);
                    break;
                case "#invoice":
                    // parse for invoice and products 'Faktura og faktura linjer'
                    ParseInvoiceRow(columns);
                    break;
            }
        }

        return Invoices.Count;
    }

}

public class Metadata
{
    public int CustomerNumberColumn { get; set; }

    private IList<ProductMetadata>? _productMetadata;

    public IList<ProductMetadata> ProductMetadata => _productMetadata ??= new List<ProductMetadata>();
}

public class ProductMetadata
{
    public int Column { get; set; }

    public string InvoiceLineText { get; set; } = string.Empty;

    // TODO not used.
    public string UnitText { get; set; } = string.Empty;

    public double UnitPrice { get; set; }

    public int UnitNumber { get; set; }

    public string ProductId { get; set; }
}
