using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

internal static class JsonSerializerOptionsCache
{
    private static JsonSerializerOptions? cachedOptions = null;

    internal static JsonSerializerOptions Options => cachedOptions ??= new JsonSerializerOptions();
}
