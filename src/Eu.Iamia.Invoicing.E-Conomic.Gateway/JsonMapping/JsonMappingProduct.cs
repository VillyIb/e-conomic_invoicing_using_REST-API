using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.JsonMapping;
public partial class JsonMappingBase
{
    public async Task<IEnumerable<Collection>> GetAllProducts(CancellationToken cancellationToken)
    {
        const int pageSize = 20;
        var serializer = new SerializerProductsHandle(new JsonSerializerFacade());

        var page = 0;
        var products = new List<Collection>();

        ProductsHandle? productsHandle = null;
        do
        {
            productsHandle = await serializer.DeserializeAsync(
                await _restMapping.GetProductsPaged(page++, pageSize, cancellationToken),
                cancellationToken
            );

            products.AddRange(productsHandle.collection);

        } while (productsHandle.collection.Count > 0);

        return products;
    }
}
