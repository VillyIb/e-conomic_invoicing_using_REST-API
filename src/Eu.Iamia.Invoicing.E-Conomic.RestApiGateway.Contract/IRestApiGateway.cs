using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

public interface IRestApiGateway
{
    Task<Stream> GetDraftInvoices(
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<Stream> GetDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    );

    Task<Stream> GetBookedInvoices
    (
        int page,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken
    );

    Task<Stream> PushInvoice(
        StringContent content,
        CancellationToken cancellationToken
    );

    Task<Stream> GetCustomersPaged(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken
    );

    Task<Stream> GetProductsPaged(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken
    );

    Task<Stream> GetProduct(
        int productNumber,
        CancellationToken cancellationToken
    );

    Task<Stream> GetBookedInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    );
}