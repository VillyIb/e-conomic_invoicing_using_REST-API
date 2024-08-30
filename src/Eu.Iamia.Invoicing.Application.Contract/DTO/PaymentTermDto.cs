namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class PaymentTermDto : ValueObject<PaymentTermDto>
{
    public string PaymentTermNumber { get; set; } = "0";

    public int DaysOfCredit { get; set; }

    public string? Name  { get; set; }

    public string PaymentTermsType { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{nameof(PaymentTermNumber)}: {PaymentTermNumber}, {nameof(Name)}: {Name} ";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PaymentTermNumber;
        yield return DaysOfCredit;
        yield return Name;
        yield return PaymentTermsType;
    }
}