namespace Eu.Iamia.Utils.Contract;

public interface IJsonSerializerFacade
{
    TValue Deserialize<TValue>(string json) where TValue : new();

    Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default) where TValue : new();
}