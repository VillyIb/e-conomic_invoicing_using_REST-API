namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Utils;

public interface IJsonSerializerFacade
{
    TValue Deserialize<TValue>(string json);

    Task<TValue> DeserializeAsync<TValue>(Stream utf8Json, CancellationToken cancellationToken = default);
}