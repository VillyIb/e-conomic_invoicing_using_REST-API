using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

public interface IEconomicGatewayV2
{
    Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IDraftInvoice?> PushInvoice(Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken);
}
