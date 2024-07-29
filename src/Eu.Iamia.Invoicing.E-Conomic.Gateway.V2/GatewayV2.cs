using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;
public class GatewayV2 : IEconomicGateway
{
    private readonly SettingsForEConomicGateway _settings;
    private readonly ICustomerReport _report;

    public GatewayV2(
        SettingsForEConomicGateway settings,
        ICustomerReport report
    )
    {
        _settings = settings;
        _report = report;
    }

    public GatewayV2(
        IOptions<SettingsForEConomicGateway> settings,
        ICustomerReport report
    ) : this(settings.Value, report)
    { }


    public Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        throw new NotImplementedException();
    }

    public Task LoadProductCache()
    {
        throw new NotImplementedException();
    }
}
