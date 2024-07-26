using System.Text.Json.Serialization;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Guards;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

public class InvoiceComparer : IEqualityComparer<Invoice>
{
    public bool Equals(Invoice? x, Invoice? y)
    {
        if (x == null && y == null) return true;
        if (x == null && y != null) return false;
        if (x != null && y == null) return false;

        return x == y;
    }

    public int GetHashCode(Invoice obj)
    {
        return 0;
    }
}

/// <summary>
/// 
/// </summary>
/// <see cref="https://restapi.e-conomic.com/schema/invoices.drafts.post.schema.json"/>
/// see:https://restdocs.e-conomic.com/#post-invoices-drafts
public class Invoice : ValueObject<Invoice>
{
    /// <summary>
    /// Invoice issue date. Format according to ISO-8601 (YYYY-MM-DD).
    /// </summary>
    /// <remarks>Required</remarks>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// The ISO 4217 3-letter currency code of the invoice.
    /// </summary>
    /// <remarks>Required</remarks>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; } = "DKK";

    /// <summary>
    /// The desired exchange rate between the invoice currency and the base currency of the agreement.
    /// The exchange rate expresses how much it will cost in base currency to buy 100 units of the invoice currency.
    /// If no exchange rate is supplied, the system will get the current daily rate,
    /// unless the invoice currency is the same as the base currency, in which case it will be set to 100.
    /// </summary>
    [JsonPropertyName("exchangeRate")]
    public double ExchangeRate { get; init; } = 100D;

    /// <summary>
    /// The total invoice amount in the invoice currency before all taxes and discounts have been applied. For a credit note this amount will be negative.
    /// </summary>
    [JsonPropertyName("netAmount")]
    public double NetAmount { get; init; }

    /// <summary>
    /// The total invoice amount in the invoice currency after all taxes and discounts have been applied. For a credit note this amount will be negative.
    /// </summary>
    [JsonPropertyName("grossAmount")]
    public double GrossAmount { get; init; }

    /// <summary>
    /// The difference between the cost price of the items on the invoice and the sales net invoice amount in base currency.
    /// For a credit note this amount will be negative.
    /// </summary>
    [JsonPropertyName("marginInBaseCurrency")]
    public double MarginInBaseCurrency { get; init; }

    [JsonPropertyName("marginPercentage")]
    public double MarginPercentage { get; init; }

    [JsonPropertyName("vatAmount")]
    public double VatAmount { get; init; }

    [JsonPropertyName("roundingAmount")]
    public double RoundingAmount { get; init; }

    /// <summary>
    /// The total cost of the items on the invoice in the base currency of the agreement.
    /// </summary>
    [JsonPropertyName("costPriceInBaseCurrency")]
    public double CostPriceInBaseCurrency { get; init; }

    /// <remarks>Required</remarks>
    [JsonPropertyName("paymentTerms")]
    public PaymentTerms? PaymentTerms { get; init; }

    /// <summary>
    /// The customer being invoiced.
    /// </summary>
    /// <remarks>Required</remarks>
    [JsonPropertyName("customer")]
    public Customer? Customer { get; init; }

    /// <remarks>Required</remarks>
    [JsonPropertyName("recipient")]
    public Recipient? Recipient { get; init; }

    [JsonPropertyName("delivery")]
    public Delivery? Delivery { get; init; }

    [JsonPropertyName("references")]
    public References? References { get; init; }

    /// <remarks>Required</remarks>
    [JsonPropertyName("layout")]
    public Layout? Layout { get; init; }

    private List<Line>? _lines;

    [JsonPropertyName("lines")]
    public List<Line> Lines
    {
        get => _lines ??= new();
        init => _lines = value;
    }

    [JsonPropertyName("notes")]
    public Notes? Notes { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CostPriceInBaseCurrency;
        yield return Currency;
        yield return Customer;
        yield return Date;
        yield return Delivery;
        yield return ExchangeRate;
        yield return GrossAmount;
        yield return Layout;
        foreach (var line in Lines)
        {
            yield return line;
        }
        yield return MarginInBaseCurrency;
        yield return MarginPercentage;
        yield return NetAmount;
        yield return Notes;
        yield return PaymentTerms;
        yield return Recipient;
        yield return References;
        yield return RoundingAmount;
        yield return VatAmount;
    }
}

public class Notes : ValueObject<Notes>
{
    /// <summary>
    /// Max 250 characters
    /// </summary>
    [JsonPropertyName("heading")]
    public string? Heading { get; set; }

    /// <summary>
    /// Max 1000 characters.
    /// </summary>
    [JsonPropertyName("textLine1")]
    public string? TextLine1 { get; set; }

    /// <summary>
    /// Max 1000 characters
    /// </summary>
    [JsonPropertyName("textLine2")]
    public string? TextLine2 { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Heading;
        yield return TextLine1;
        yield return TextLine2;
    }
}

public class PaymentTerms : ValueObject<PaymentTerms>
{
    [JsonPropertyName("paymentTermsNumber")]
    public int PaymentTermsNumber { get; init; }

    [JsonPropertyName("daysOfCredit")]
    public int DaysOfCredit { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("paymentTermsType")]
    public PaymentTermsType PaymentTermsType { get; init; } = PaymentTermsType.invoiceMonth;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name ?? string.Empty;
        yield return DaysOfCredit;
        yield return PaymentTermsNumber;
        yield return PaymentTermsType;
    }
}

public enum PaymentTermsType
{
    // ReSharper disable InconsistentNaming
    [JsonPropertyName("net")]
    net,
    [JsonPropertyName("invoiceMonth")]
    invoiceMonth,
    [JsonPropertyName("paidInCash")]
    paidInCash,
    [JsonPropertyName("prepaid")]
    prepaid,
    [JsonPropertyName("dueDate")]
    dueDate,
    [JsonPropertyName("factoring")]
    factoring,
    [JsonPropertyName("invoiceWeekStartingSunday")]
    invoiceWeekStartingSunday,
    [JsonPropertyName("invoiceWeekStartingMonday")]
    invoiceWeekStartingMonday,
    [JsonPropertyName("creditcard")]
    creditcard,
    [JsonPropertyName("avtaleGiro")]
    avtaleGiro
    // ReSharper restore InconsistentNaming
}

public class Customer : ValueObject<Customer>
{
    /// <summary>
    /// The customer id number. The customer id number can be either positive or negative, but it can’t be zero.
    /// </summary>
    [JsonPropertyName("customerNumber")]
    public int CustomerNumber { get; init; }

    /// <summary>
    /// A unique reference to the customer resource.
    /// </summary>
    [JsonPropertyName("self")]
    public string? Self { get; init; }

    public Customer(int customerNumber, string? self = null)
    {
        InvoiceGuards.CustomerNumber(customerNumber);

        CustomerNumber = customerNumber;
        Self = self;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CustomerNumber;
        yield return Self;
    }
}

public class Recipient : ValueObject<Recipient>
{
    /// <remarks>Required</remarks>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("zip")]
    public string? Zip { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <remarks>Required</remarks>
    [JsonPropertyName("vatZone")]
    public VatZone? VatZone { get; init; }

    public static Recipient NullRecipient => new();
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name ?? string.Empty;
        yield return City ?? string.Empty;
        yield return VatZone ?? VatZone.NullVatZone;
        yield return Zip ?? string.Empty;
    }
}

public class VatZone : ValueObject<VatZone>
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("vatZoneNumber")]
    public int VatZoneNumber { get; init; }

    [JsonPropertyName("enabledForCustomer")]
    public bool EnabledForCustomer { get; init; }

    [JsonPropertyName("enabledForSupplier")]
    public bool EnabledForSupplier { get; init; }

    public static VatZone? NullVatZone => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name ?? string.Empty;
        yield return EnabledForCustomer;
        yield return EnabledForSupplier;
        yield return VatZoneNumber;
    }
}

public class Delivery : ValueObject<Delivery>
{
    /// <summary>
    /// Required as public for deserialization.
    /// </summary>
    public Delivery() : this(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Today)
    { }

    public Delivery(string? address
        , string? zip
        , string? city
        , string? country
        //, DateTime deliveryDate
        , DateTime deliveryDate
    )
    {
        InvoiceGuards.DeliveryAddress(address);
        InvoiceGuards.DeliveryZip(zip);
        InvoiceGuards.DeliveryCity(city);
        InvoiceGuards.DeliveryCountry(country);

        Address = address;
        Zip = zip;
        City = city;
        Country = country;
        DeliveryDate = deliveryDate.ToString("yyyy-MM-dd");
    }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("zip")]
    public string? Zip { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("deliveryDate")]
    public string? DeliveryDate { get; init; }

    public static Delivery NullDelivery => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Address;
        yield return City;
        yield return Country;
        yield return DeliveryDate;
        yield return Zip;
    }
}

public class References : ValueObject<References>
{
    [JsonPropertyName("other")]
    public string? Other { get; init; }

    public static References NullReferences => new();
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Other ?? string.Empty;
    }
}

public class Layout : ValueObject<Layout>
{
    [JsonPropertyName("layoutNumber")]
    public int LayoutNumber { get; init; }

    public static Layout NullLayout => new();
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LayoutNumber;
    }
}

public class Line : ValueObject<Line>
{
    [JsonPropertyName("lineNumber")]
    public int LineNumber { get; init; }

    [JsonPropertyName("sortKey")]
    public int SortKey { get; init; }

    [JsonPropertyName("unit")]
    public Unit? Unit { get; init; }

    [JsonPropertyName("product")]
    public Product? Product { get; init; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; init; }

    [JsonPropertyName("unitNetPrice")]
    public double? UnitNetPrice { get; init; }

    [JsonPropertyName("discountPercentage")]
    public double? DiscountPercentage { get; init; }

    [JsonPropertyName("unitCostPrice")]
    public double? UnitCostPrice { get; init; }

    [JsonPropertyName("totalNetAmount")]
    public double TotalNetAmount { get; init; }

    [JsonPropertyName("marginInBaseCurrency")]
    public double? MarginInBaseCurrency { get; init; }

    [JsonPropertyName("marginPercentage")]
    public double? MarginPercentage { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Description;
        yield return DiscountPercentage ?? 0.0;
        yield return LineNumber;
        yield return MarginInBaseCurrency ?? 0.0;
        yield return MarginPercentage ?? 0.0;
        yield return Product ?? Product.NullProduct;
        yield return Quantity;
        yield return SortKey;
        yield return Unit ?? Unit.NullUnit;
        yield return UnitCostPrice ?? 0.0;
        yield return UnitNetPrice ?? 0.0;
        yield return TotalNetAmount;
    }
}

public class Unit : ValueObject<Unit>
{
    public Unit()
    {
    }

    public Unit(string? name, int unitNumber)
    {
        Name = name;
        UnitNumber = unitNumber;
    }

    [JsonPropertyName("unitNumber")]
    public int UnitNumber { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    public static Unit NullUnit => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return UnitNumber;
        yield return Name ?? string.Empty;
    }
}

public class Product : ValueObject<Product>
{
    [JsonPropertyName("productNumber")]
    public string? ProductNumber { get; init; }

    public static Product? NullProduct => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductNumber ?? string.Empty;
    }
}

public static class InvoiceExtension
{
    public static string ToJson(this Invoice invoice)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var json = JsonSerializer.Serialize(invoice, options);
        return json;
    }

    public static Invoice? FromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        };

        var invoice = JsonSerializer.Deserialize<Invoice>(json, options);
        return invoice;
    }
}