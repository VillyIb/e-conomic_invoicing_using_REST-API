using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    [Obsolete]
    public async Task<string> ReadProductsPaged(int page, int pageSize)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync($"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("ReadProductsPaged", htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (HttpRequestException ex)
        {
            return ex.StatusCode.ToString() ?? string.Empty;
        }
    }

    public async Task<ProductsHandle>? ReadProductsPaged2(int page, int pageSize)
    {
        try
        {
            SetAuthenticationHeaders();

            // ReSharper disable StringLiteralTypo
            var response =
                await _httpClient.GetAsync(
                    $"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}");
            // ReSharper restore StringLiteralTypo

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("ReadProductsPaged", htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var json = await GetHtmlBody(response);
            var productsHandle = ProductsHandleExtension.FromJson(json);
            return productsHandle!;
        }
        catch (HttpRequestException ex)
        {
            Report.Error(nameof(ReadProductsPaged2), ex.Message);
            return new ProductsHandle
            {
                collection = new List<Collection>(0),
            };
        }
        catch (JsonException ex)
        {
            Report.Error(nameof(ReadProductsPaged2), ex.Message);
            return new ProductsHandle
            {
                collection = new List<Collection>(0),
            };
        }
    }
}
