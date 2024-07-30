namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class InvoiceLineDto : ValueObject<InvoiceLineDto>
{
    public string? UnitText { get; set; }

    public int UnitNumber { get; set; }

    public string? ProductNumber { get; set; }

    public double? Quantity { get; set; }

    public double? UnitNetPrice { get; set; }

    public string? Description { get; set; }

    public int? SourceFileLineNumber { get; set; }

    //public int SourceFileLine { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return UnitNumber;
        yield return ProductNumber ?? string.Empty;
        yield return Quantity ?? 0;
        yield return UnitNetPrice ?? 0;
        yield return Description ?? string.Empty;
        yield return SourceFileLineNumber ?? 0;
    }
}