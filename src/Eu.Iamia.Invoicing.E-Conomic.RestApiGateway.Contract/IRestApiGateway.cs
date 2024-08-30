using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

public interface IRestApiGateway
{
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

    Task<Stream> PushInvoice(
        StringContent content,
        CancellationToken cancellationToken
    );

    Task<Stream> GetBookedInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    );

    #endregion

    #region customer

    Task<Stream> GetCustomersPaged(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );

    #endregion

    #region Product

    Task<Stream> GetProductsPaged(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<Stream> GetProduct(
        int productNumber,
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

}