namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;

using Serializers;
using Contract.DTO.Invoices.drafts.draftInvoiceNumber.get;

public partial class GatewayV2
{
    public async Task<DraftInvoice?> GetDraftInvoice(int draftInvoiceNumber, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetDraftInvoice(draftInvoiceNumber, cancellationToken);

        var draftInvoice = await GenericSerializer<DraftInvoice>.DeserializeAsync(stream, cancellationToken);

        return draftInvoice ?? new();
    }
}
