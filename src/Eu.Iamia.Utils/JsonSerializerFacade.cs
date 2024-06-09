using System.Text.Json;

namespace Eu.Iamia.Utils;

public interface IJsonSerializerFacade
{
    TValue Deserialize<TValue>(string json);

    Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default);
}

public class JsonSerializerFacade : IJsonSerializerFacade
{
    private static readonly JsonSerializerOptions Options = new();

    protected virtual TValue? Deserialize<TValue>(string json, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TValue>(json, options);
    }

    protected virtual async ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<TValue>(utf8Json, options, cancellationToken);
    }

    public TValue Deserialize<TValue>(string json)
    {
        try
        {
            var value = Deserialize<TValue>(json, Options);
            if (value is null)
            {
                throw new JsonException("result is null");
            }
            return value;
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

    public async Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default)
    {
        try
        {
            var value = await DeserializeAsync<TValue>(utf8Json, Options, cancellationToken);
            if (value is null)
            {
                throw new JsonException("result is null");
            }
            return value;
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
