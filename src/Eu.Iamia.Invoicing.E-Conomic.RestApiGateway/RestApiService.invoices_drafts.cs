// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public async Task<Stream> GetDraftInvoices(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts

        // see: https://restapi.e-conomic.com/schema/invoices.drafts.get.schema.json

        const string reference = nameof(GetDraftInvoices);

        var requestUri =
            $"https://restapi.e-conomic.com/invoices/drafts?" +
            $"skippages={skipPages}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts-draftinvoicenumber

        // see:https://restapi.e-conomic.com/schema/invoices.drafts.draftInvoiceNumber.get.schema.json

        const string reference = nameof(GetDraftInvoice);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> DeleteDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken = default
    )
    {
        // see: https://restdocs.e-conomic.com/#delete-invoices-drafts

        const string reference = nameof(DeleteDraftInvoice);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await DeleteAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> PostDraftInvoice(
        StringContent content,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#post-invoices-drafts-draftinvoicenumber-lines

        // see: https://restapi.e-conomic.com/schema/invoices.drafts.draftInvoiceNumber.lines.post.schema.json

        const string reference = nameof(PostDraftInvoice);

        var requestUri = $"https://restapi.e-conomic.com//invoices/drafts";
        return await PostAsync(requestUri, content, reference, cancellationToken);
    }
}
