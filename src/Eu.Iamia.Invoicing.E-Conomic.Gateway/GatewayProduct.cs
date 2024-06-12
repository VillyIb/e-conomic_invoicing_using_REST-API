using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    public async Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        const string reference = nameof(ReadProductsPaged);

        SetAuthenticationHeaders();

        // ReSharper disable StringLiteralTypo
        var response =
            await HttpClient.GetAsync(
                $"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}", cancellationToken);
        // ReSharper restore StringLiteralTypo

        if (!response.IsSuccessStatusCode)
        {
            var htmlBodyFail = await GetHtmlBody(response);
            Report.Error(reference, htmlBodyFail);

            response.EnsureSuccessStatusCode();
        }

        const HttpStatusCode expected = HttpStatusCode.OK;
        if (expected != response.StatusCode)
        {
            var message = $@"Response status code does not indicate {expected}: {response.StatusCode:D} ({response.ReasonPhrase})";
            Report.Error(reference, message);
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        var productsHandle = await SerializerProductsHandle.DeserializeAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken
        );

        return productsHandle;
    }
}
