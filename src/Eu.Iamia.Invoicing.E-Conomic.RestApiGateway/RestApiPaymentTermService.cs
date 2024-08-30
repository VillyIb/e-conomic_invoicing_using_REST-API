// ReSharper disable StringLiteralTypo
namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public async Task<Stream> GetPaymentTerms(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        const string reference = nameof(GetPaymentTerms);

        var requestUri =
                $"https://restapi.e-conomic.com/payment-terms?" +
                $"skippages={skipPages}&pagesize={pageSize}"
            ;

        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
