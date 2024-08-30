// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public virtual async Task<Stream> GetCustomersPaged(int skipPages, int pageSize, CancellationToken cancellationToken)
    {
        // see:https://restdocs.e-conomic.com/#get-customers

        const string reference = nameof(GetCustomersPaged);

        var requestUri =
            $"https://restapi.e-conomic.com/customers?" +
            $"skippages={skipPages}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
