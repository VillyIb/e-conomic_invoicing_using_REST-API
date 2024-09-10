using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

public interface IRestApiGateway
{
    #region Customer

    Task<Stream> GetCustomers(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );

    #endregion

    #region Invoice

    Task<Stream> GetDraftInvoices(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<Stream> GetDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    );

    Task<Stream> GetBookedInvoices
    (
        int skipPages,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken
    );

    Task<Stream> GetBookedInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    );

    Task<Stream> PostInvoice(
        StringContent content,
        CancellationToken cancellationToken
    );

    #endregion

    #region PaymentTerm

    Task<Stream> GetPaymentTerms(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );


    #endregion

    #region Product

    Task<Stream> GetProducts(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<Stream> GetProduct(
        int productNumber,
        CancellationToken cancellationToken
    );

    #endregion

}