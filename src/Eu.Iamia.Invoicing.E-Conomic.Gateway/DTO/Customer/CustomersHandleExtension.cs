using System.Text.Json;
using System.Text.Json.Serialization;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

#pragma warning disable CS8618
public static class CustomersHandleExtension
{
    public static string ToJson(this CustomersHandle customersHandle)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var json = JsonSerializer.Serialize(customersHandle, options);
        return json;
    }

    public static CustomersHandle? FromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var invoice = JsonSerializer.Deserialize<CustomersHandle>(json, options);
        return invoice;
    }

}