namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public  class ProductDto : ValueObject<ProductDto>
{
    public string Description { get; init; } = string.Empty;

    public UnitDto? Unit { get; init; }

    public string Name { get; init; } = string.Empty;

    public string ProductNumber { get; init; } = string.Empty;

    public override string ToString()
    {
        return $"{nameof(ProductNumber)}: {ProductNumber}, {nameof(Name)}: {Name} ";
    }

    public class UnitDto: ValueObject<UnitDto>
    {
        public string Name { get; init; } = string.Empty;

        public int UnitNumber { get; init; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return UnitNumber;
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Description;
        yield return Unit;
        yield return Name;
        yield return ProductNumber;
    }
}
