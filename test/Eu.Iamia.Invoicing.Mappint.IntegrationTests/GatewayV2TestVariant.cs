using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.Mapping.IntegrationTests;

/// <summary>
/// Clean up draft invoices after test.
/// </summary>
public  class GatewayV2TestVariant : GatewayV2
{
    public GatewayV2TestVariant(SettingsForEConomicGatewayV2 settings, IRestApiGateway restApiGateway, ICustomerReport report) : base(settings, restApiGateway, report)
    { }

    public GatewayV2TestVariant(IOptions<SettingsForEConomicGatewayV2> settings, IRestApiGateway restApiGateway, ICustomerReport report) : base(settings, restApiGateway, report)
    { }

    public Task<int> DeleteDraftInvoices(int customerNumber)
    {
        // get draft invoices
        // select invoices matching customerNumber
        // loop
        // delete draft invoice

        return Task.FromResult(0);
    }
}
