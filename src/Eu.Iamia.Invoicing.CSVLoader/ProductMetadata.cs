// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.CSVLoader;

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
