namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

public interface IEconomicGatewayV2
{
    Task<DTO.Customer.CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<DTO.Product.ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<IDraftInvoice?> PushInvoice(Application.Contract.DTO.InvoiceDto invoice, int sourceFileNumber, CancellationToken cancellationToken);

    Task<IDraftInvoice?> PushInvoice(DTO.Invoice.Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken);
}
