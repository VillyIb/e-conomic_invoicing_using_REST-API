using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace Eu.Iamia.Utils;
public static class JsonSerializerFacade
{
    // public static TValue? Deserialize<TValue>([StringSyntax(StringSyntaxAttribute.Json)] string json, JsonSerializerOptions? options = null)

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static TValue Deserialize<TValue>([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                Converters = { new JsonStringEnumConverter() }
            };

            var value = JsonSerializer.Deserialize<TValue>(json, options);
            return value is not null 
                ? value
                : throw new JsonException()
            ;
        }
        catch (JsonException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var type = ex.GetType().Name;
            throw new JsonException(type, ex);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="utf8Json"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<TValue> DeserializeAsync<TValue>(Stream utf8Json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                Converters = { new JsonStringEnumConverter() }
            };

            var value = await JsonSerializer.DeserializeAsync<TValue>(utf8Json, options);
            return value is not null
                    ? value
                    : throw new JsonException()
                ;
        }
        catch (JsonException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var type = ex.GetType().Name;
            throw new JsonException(type, ex);
        }
    }
}
