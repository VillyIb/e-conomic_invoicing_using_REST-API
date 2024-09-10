// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public virtual async Task<Stream> GetCustomers(int skipPages, int pageSize, CancellationToken cancellationToken)
    {
        // see:https://restdocs.e-conomic.com/#get-customers

        // see: https://restapi.e-conomic.com/schema/customers.get.schema.json

        const string reference = nameof(GetCustomers);

        var requestUri =
            $"https://restapi.e-conomic.com/customers?" +
            $"skippages={skipPages}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUri, reference, cancellationToken);
    }
}
