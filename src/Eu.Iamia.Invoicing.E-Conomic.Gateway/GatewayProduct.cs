﻿using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    [Obsolete]
    public async Task<string> ReadProductsPagedObsolete(int page, int pageSize)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await HttpClient.GetAsync($"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}");

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

    public async Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            SetAuthenticationHeaders();

            // ReSharper disable StringLiteralTypo
            var response =
                await HttpClient.GetAsync(
                    $"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}", cancellationToken);
            // ReSharper restore StringLiteralTypo

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(nameof(ReadProductsPaged), htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var productsHandle = await SerializerProductsHandle.DeserializeAsync(
                await response.Content.ReadAsStreamAsync(cancellationToken),
                cancellationToken
            );

            return productsHandle;
        }
        catch (HttpRequestException)
        {
            return new ProductsHandle
            {
                collection = new List<Collection>(0),
            };
        }
        catch (JsonException ex)
        {
            Report.Error(nameof(ReadProductsPaged), ex.Message);
            return new ProductsHandle
            {
                collection = new List<Collection>(0),
            };
        }
    }
}
