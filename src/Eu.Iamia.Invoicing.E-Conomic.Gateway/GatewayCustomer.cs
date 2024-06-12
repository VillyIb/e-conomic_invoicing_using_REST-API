using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using System.Text.Json;
using Collection = Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer.Collection;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // https://restapi.e-conomic.com/customers?skippages=0&pagesize=20

    public async Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await HttpClient.GetAsync($"https://restapi.e-conomic.com/customers?skippages={page}&pagesize={pageSize}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(nameof(ReadCustomersPaged), htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var customersHandle = await SerializerCustomersHandle.DeserializeAsync(
                await response.Content.ReadAsStreamAsync(cancellationToken),
                cancellationToken
            );

            return customersHandle;
        }
        catch (HttpRequestException ex)
        {
            var name = ex.GetType().Name;

            return new CustomersHandle()
            {
                collection = new List<Collection>(0)
            };
        }
        catch (JsonException ex)
        {
            Report.Error(nameof(ReadCustomersPaged), ex.Message);
            return new CustomersHandle()
            {
                collection = new List<Collection>(0)
            };
        }
    }
}
