using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

// TODO still used move..
public class SerializerProductsHandle : ISerializerProductsHandle
{
    private readonly IJsonSerializerFacade _serializer;

    public SerializerProductsHandle(IJsonSerializerFacade serializer)
    {
        _serializer = serializer;
    }

    public async Task<ProductsHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await _serializer.DeserializeAsync<ProductsHandle>(utf8Json, cancellationToken);
    }
}
