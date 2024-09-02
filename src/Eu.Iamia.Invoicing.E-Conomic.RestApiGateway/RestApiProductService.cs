
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public virtual async Task<Stream> GetProducts(int skipPages, int pageSize, CancellationToken cancellationToken)
    {
        // see: https://restdocs.e-conomic.com/#get-products

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
        // see: https://restdocs.e-conomic.com/#get-invoices-booked-bookedinvoicenumber

        const string reference = nameof(GetBookedInvoices);

        var requestUri = $"https://restapi.e-conomic.com/products/{productNumber}";
        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
