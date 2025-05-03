
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public virtual async Task<Stream> GetProducts(int skipPages, int pageSize, CancellationToken cancellationToken)
    {
        // see: https://restdocs.e-conomic.com/#get-products

        // see: https://restapi.e-conomic.com/schema/products.get.schema.json

        const string reference = nameof(GetProducts);

        var requestUrl =
            $"https://restapi.e-conomic.com/products?" +
            $"skippages={skipPages}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUrl, reference, cancellationToken);
    }

    public async Task<Stream> GetProduct(
        int productNumber,
        CancellationToken cancellationToken
    )
    {
        // ReSharper disable once CommentTypo

        // see: https://restdocs.e-conomic.com/#get-products-productnumber

        // see: https://restapi.e-conomic.com/schema/products.productNumber.get.schema.json

        const string reference = nameof(GetProduct);

        var requestUri = $"https://restapi.e-conomic.com/products/{productNumber}";
        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
