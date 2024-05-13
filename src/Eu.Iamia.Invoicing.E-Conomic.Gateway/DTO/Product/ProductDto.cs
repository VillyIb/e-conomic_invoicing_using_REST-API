using System.Text.Json.Serialization;
using System.Text.Json;
using Eu.Iamia.Utils;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
#pragma warning disable CS8618

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
internal class ProductDto
{
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Collection
{
    public string productNumber { get; set; }
    public string description { get; set; }
    public string name { get; set; }
    public double salesPrice { get; set; }
    public bool barred { get; set; }
    public DateTime lastUpdated { get; set; }
    public ProductGroup productGroup { get; set; }
    public Invoices invoices { get; set; }
    public Pricing pricing { get; set; }
    public string self { get; set; }
    public double? recommendedPrice { get; set; }
    public Unit unit { get; set; }
    public double? minimumStock { get; set; }
}

public class Create
{
    public string description { get; set; }
    public string href { get; set; }
    public string httpMethod { get; set; }
}

public class Invoices
{
    public string drafts { get; set; }
    public string booked { get; set; }
    public string self { get; set; }
}

public class MetaData
{
    public Create create { get; set; }
}

public class Pagination
{
    public int skipPages { get; set; }
    public int pageSize { get; set; }
    public int maxPageSizeAllowed { get; set; }
    public int results { get; set; }
    public int resultsWithoutFilter { get; set; }
    public string firstPage { get; set; }
    public string nextPage { get; set; }
    public string lastPage { get; set; }
}

public class Pricing
{
    public string currencySpecificSalesPrices { get; set; }
}

public class ProductGroup
{
    public int productGroupNumber { get; set; }
    public string name { get; set; }
    public string salesAccounts { get; set; }
    public string products { get; set; }
    public string self { get; set; }
}

public class ProductsHandle
{
    public List<Collection> collection { get; set; }
    public Pagination pagination { get; set; }
    public MetaData metaData { get; set; }
    public string self { get; set; }
}

public class Unit
{
    public int unitNumber { get; set; }
    public string name { get; set; }
    public string products { get; set; }
    public string self { get; set; }
}


public static class ProductsHandleExtension
{
    public static string ToJson(this ProductsHandle productsHandle)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var json = JsonSerializer.Serialize(productsHandle, options);
        return json;
    }

    /// <summary>
    /// Returns deserialized object.
    /// </summary>
    /// <param name="json">Json formatted string</param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static ProductsHandle FromJson(string json)
    {
        var invoice = JsonSerializerFacade.Deserialize<ProductsHandle>(json);
        return invoice;
    }

}