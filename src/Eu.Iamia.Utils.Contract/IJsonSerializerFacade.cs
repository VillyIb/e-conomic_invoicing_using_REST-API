namespace Eu.Iamia.Utils.Contract;

public interface IJsonSerializerFacade
{
    TValue Deserialize<TValue>(string json);

    Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default);
}