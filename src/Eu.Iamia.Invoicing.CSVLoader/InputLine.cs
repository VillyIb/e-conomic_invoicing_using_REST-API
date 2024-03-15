using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;

public class InputLine : IInputLine
{
    public Units? UnitNumber { get; set; }

    public Products? ProductNumber { get; set; }

    public double? Quantity { get; set; }

    public double? UnitNetPrice { get; set; }

    public string? Description { get; set; }
}