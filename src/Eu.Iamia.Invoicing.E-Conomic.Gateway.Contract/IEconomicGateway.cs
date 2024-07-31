using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

using Eu.Iamia.Invoicing.Loader.Contract;

public interface IEconomicGateway
{
    //Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    //Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// PushInvoice contract.
    /// </summary>
    /// <param name="inputInvoice"></param>
    /// <param name="sourceFileLineNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    Task<IDraftInvoice?> PushInvoice(Application.Contract.DTO.InvoiceDto inputInvoice, int sourceFileLineNumber, CancellationToken cancellationToken);

    Task LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task LoadProductCache();
}

public interface IDraftInvoice
{
    int DraftInvoiceNumber { get; }

    double GrossAmount { get; }
}