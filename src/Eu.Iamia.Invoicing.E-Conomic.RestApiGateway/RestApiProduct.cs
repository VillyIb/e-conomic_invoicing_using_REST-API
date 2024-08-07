﻿
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiBase
{
    public virtual async Task<Stream> GetProductsPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        // see: https://restdocs.e-conomic.com/#get-products

        const string reference = nameof(GetProductsPaged);

        var requestUrl =
            $"https://restapi.e-conomic.com/products?" +
            $"skippages={page}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUrl, reference, cancellationToken);
    }

    public async Task<Stream> GetProduct(
        int productNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked-bookedinvoicenumber

        const string reference = nameof(GetBookedInvoices);

        var requestUri = $"https://restapi.e-conomic.com/products/{productNumber}";
        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
