using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;
public class LineParser
{
    private readonly DateTime _invoiceDate;

    public LineParser(DateTime invoiceDate)
    {
        _invoiceDate = invoiceDate;
    }

    private InputInvoice? Result { get; set; }

    private void ProcessProduct(string row, Products product)
    {
        var productStrategy = new StrategyByProduct(product, _invoiceDate);

        if (!int.TryParse(row, out var amount) || amount <= 0) return;

        var line = new InputLine();
        line.Quantity = amount;
        line.Description = productStrategy.ProductName;
        line.UnitNetPrice = productStrategy.UnitNetPrice;
        line.ProductNumber = productStrategy.Product;
        line.UnitNumber = productStrategy.Units;
        Result!.InvoiceLines.Add(line);
    }

    public InputInvoice FromString(string singleRow)
    {
        var columns = singleRow.Split(new[] { ";" }, StringSplitOptions.TrimEntries);

        if (!int.TryParse(columns[0], out var customerNo) || customerNo == 0)
        {
            throw new ArgumentException($"Customer not found: '{singleRow}'");
        }

        if (columns.Length < 21)
        {
            throw new ArgumentException($"Not enough columns {columns.Length} expected 20 '{singleRow}'");
        }

        Result = new InputInvoice
        {
            CustomerNumber = customerNo,
            InvoiceDate = _invoiceDate,
            InvoiceLines = new List<IInputLine>()
        };

        {
            ProcessProduct(columns[4], Products.Grundejerforening);
            ProcessProduct(columns[6], Products.MedlemsKontingent);
            ProcessProduct(columns[8], Products.Vandafledning);
            ProcessProduct(columns[10], Products.Jordvarme);
            ProcessProduct(columns[12], Products.LejeNyttehave);
            ProcessProduct(columns[14], Products.LejeJordkælder);
            ProcessProduct(columns[16], Products.LejeMarkskur);
            ProcessProduct(columns[18], Products.VideresalgElektricitet);
            ProcessProduct(columns[20], Products.VideresalgVand);
        }

        return Result;
    }

}
