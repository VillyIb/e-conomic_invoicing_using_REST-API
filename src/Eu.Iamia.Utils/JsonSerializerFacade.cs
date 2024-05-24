using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace Eu.Iamia.Utils;
public static class JsonSerializerFacade
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters = { new JsonStringEnumConverter() }
    };

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
            var value = JsonSerializer.Deserialize<TValue>(json, Options);
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
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> that can be used to cancel the read operation.
    /// </param>
    /// <returns>A <typeparamref name="TValue"/> representation of the JSON value.</returns>
    /// <exception cref="JsonException"></exception>
    public static async Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default)
    {
        try
        {
            var value = await JsonSerializer.DeserializeAsync<TValue>(utf8Json, Options, cancellationToken);
            return value is not null
                    ? value
                    : throw new JsonException("Deserialize returned null")
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
