using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

[Obsolete]
public class InputLine : IInputLine
{
    public string? UnitText { get; set; }

    public int UnitNumber { get; set; }

    public string? ProductNumber { get; set; }

    public double? Quantity { get; set; }

    public double? UnitNetPrice { get; set; }

    public string? Description { get; set; }

    public int? SourceFileLineNumber { get; set; }

    public int SourceFileLine { get; set; }
}