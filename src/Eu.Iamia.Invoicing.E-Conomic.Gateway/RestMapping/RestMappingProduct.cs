
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;

public partial class RestMappingBase
{
    public virtual async Task<Stream> GetProductsPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        // see: https://restdocs.e-conomic.com/#get-products

        const string reference = nameof(GetProductsPaged);

        var requestUrl = 
            $"https://restapi.e-conomic.com/products?" +
            $"skippages={page}&pagesize={pageSize}"
        ;

        return await GetAny(requestUrl, reference, cancellationToken);
    }
}
