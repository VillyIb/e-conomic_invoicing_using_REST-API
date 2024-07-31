using System.Text.Json.Serialization;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;


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

public class Unit : ValueObject<Unit>
{
    public Unit()
    {
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

    public static Product NullProduct => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductNumber ?? string.Empty;
    }
}
