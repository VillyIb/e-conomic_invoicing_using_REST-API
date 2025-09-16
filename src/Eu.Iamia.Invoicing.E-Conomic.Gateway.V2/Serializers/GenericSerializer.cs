using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public static class GenericSerializer<T> where T : class
{
    public static async Task<T?> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
#if DEBUG && false
        // Option to inspect payload of utf8Json stream.
        var reader = new StreamReader(utf8Json);
        var json = await reader.ReadToEndAsync(cancellationToken);

        utf8Json.Position = 0;
#endif
        var result = await JsonSerializer.DeserializeAsync<T>(utf8Json, JsonSerializerOptionsCache.Options, cancellationToken) ;

        return result;
    }

}
