using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

public interface IRestMappingBase
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
}