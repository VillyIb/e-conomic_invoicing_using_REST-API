using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

using Eu.Iamia.Invoicing.Loader.Contract;

public interface IEconomicGateway
{
    Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber);

    Task LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task LoadProductCache();

    Task<string> ReadInvoice();
}

public interface IDraftInvoice
{
    int DraftInvoiceNumber { get; set; }

    double GrossAmount { get; set; }
}