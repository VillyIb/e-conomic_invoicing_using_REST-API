namespace Eu.Iamia.Invoicing.Loader.Contract;

public interface IInputLine
{
    Units?    UnitNumber    { get; set; }

    Products? ProductNumber { get; set; }

    double? Quantity      { get; set; }

    double? UnitNetPrice  { get; set; }

    string? Description   { get; set; }
}