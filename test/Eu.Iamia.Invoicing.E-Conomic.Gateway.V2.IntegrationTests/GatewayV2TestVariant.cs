using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests;

/// <summary>
/// Clean up draft invoices after test.
/// </summary>
public  class GatewayV2TestVariant : GatewayV2
{
    public GatewayV2TestVariant(SettingsForEConomicGatewayV2 settings, IRestApiGateway restApiGateway, ICustomerReport report) : base(settings, restApiGateway, report)
    { }

    public GatewayV2TestVariant(IOptions<SettingsForEConomicGatewayV2> settings, IRestApiGateway restApiGateway, ICustomerReport report) : base(settings, restApiGateway, report)
    { }

    public async Task<BookedInvoicesHandle> ReadBookedInvoices(int page, int pageSize, IInterval<DateTime> dateRange, CancellationToken cancellationToken = default)
    {
        var stream = await _restApiGateway.GetBookedInvoices(page, pageSize, dateRange, cancellationToken);

        var serializerCustomersHandle = new SerializerBookedInvoicesHandle();

        var customersHandle = await serializerCustomersHandle.DeserializeAsync(stream, cancellationToken);

        return customersHandle;
    }

    public async Task<int> DeleteDraftInvoices(int customerNumber)
    {
        // get draft invoices
        // select invoices matching customerNumber
        // loop
        // delete draft invoice


        return 0;
    }
}
