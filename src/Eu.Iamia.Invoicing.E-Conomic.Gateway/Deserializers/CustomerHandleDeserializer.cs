using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.DraftInvoice;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Deserializers;

public interface ISerializerCustomersHandle
{
    Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}

public class SerializerCustomersHandle : ISerializerCustomersHandle
{
    private readonly IJsonSerializerFacadeV2 _serializer;

    public SerializerCustomersHandle(IJsonSerializerFacadeV2 serializer)
    {
        _serializer = serializer;
    }

    public async Task<CustomersHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<CustomersHandle>(utf8Json, cancellationToken);
    }
}

public interface ISerializerDraftInvoice
{
    DraftInvoice Deserialize(string json);

    Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}

public class SerializerDraftInvoice : ISerializerDraftInvoice
{
    private readonly IJsonSerializerFacadeV2 _serializer;

    public SerializerDraftInvoice(IJsonSerializerFacadeV2 serializer)
    {
        _serializer = serializer;
    }

    public DraftInvoice Deserialize(string json)
    {
        return _serializer.Deserialize<DraftInvoice>(json);
    }

    public async Task<DraftInvoice> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<DraftInvoice>(utf8Json, cancellationToken);
    }
}

public interface ISerializerProductsHandle
{
    Task<ProductsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken);
}

public class SerializerProductsHandle : ISerializerProductsHandle
{
    private readonly IJsonSerializerFacadeV2 _serializer;

    public SerializerProductsHandle(IJsonSerializerFacadeV2 serializer)
    {
        _serializer = serializer;
    }

    public async Task<ProductsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<ProductsHandle>(utf8Json, cancellationToken);
    }
}
