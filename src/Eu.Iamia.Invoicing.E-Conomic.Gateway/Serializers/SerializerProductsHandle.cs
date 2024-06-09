using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;

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
