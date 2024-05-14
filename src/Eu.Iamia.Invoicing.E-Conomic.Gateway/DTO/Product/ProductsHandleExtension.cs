using System.Text.Json;
using System.Text.Json.Serialization;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

#pragma warning disable CS8618
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