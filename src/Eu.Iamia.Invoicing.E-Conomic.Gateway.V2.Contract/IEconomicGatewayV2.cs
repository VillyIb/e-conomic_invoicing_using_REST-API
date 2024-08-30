using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DraftInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

public interface IEconomicGatewayV2
{
    Task<DTO.Customer.CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<DTO.Product.ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<DTO.PaymentTerm.PaymentTermsHandle?> ReadPaymentTermsPaged(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IDraftInvoice?> PushInvoice(DTO.Invoice.Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken);

    Task<int> LoadPaymentTermsCache();

    Contract.DTO.PaymentTerm.PaymentTerm? GetPaymentTerm(int paymentTermsNumber);
}
