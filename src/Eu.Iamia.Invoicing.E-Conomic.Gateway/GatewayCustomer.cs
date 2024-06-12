using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using System.Net;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // https://restapi.e-conomic.com/customers?skippages=0&pagesize=20

    public async Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        const string reference = nameof(ReadCustomersPaged);

        SetAuthenticationHeaders();

        // ReSharper disable once StringLiteralTypo
        var response = await HttpClient.GetAsync($"https://restapi.e-conomic.com/customers?skippages={page}&pagesize={pageSize}", cancellationToken);

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

        var customersHandle = await SerializerCustomersHandle.DeserializeAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken
        );

        return customersHandle;
    }
}
