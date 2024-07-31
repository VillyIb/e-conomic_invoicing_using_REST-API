namespace Eu.Iamia.Invoicing.Loader.Contract;

[Obsolete("Use Application.Contract.DTO.InvoiceLineDto", true)]
public interface IInputLine
{
    string? UnitText { get; set; }

    string? ProductNumber { get; set; }

    int UnitNumber { get; set; }

    double? Quantity { get; set; }

    double? UnitNetPrice { get; set; }

    string? Description { get; set; }

    int? SourceFileLineNumber { get; set; }
}