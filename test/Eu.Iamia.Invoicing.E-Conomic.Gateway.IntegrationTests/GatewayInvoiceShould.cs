using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class GatewayInvoiceShould
{
    private readonly GatewayBase _sut;

    public GatewayInvoiceShould()
    {
        using var setup = new Setup();
        _sut = (GatewayBase)setup.GetService<IEconomicGateway>();
    }

    [Fact]
    public async Task PushInvoice_When_ValidInvoice_DraftInvoice_Exists()
    {
        // Notice Creates a real invoice in e-conomic

        Invoice invoiceStub = InvoiceStubExtension.Valid(CachedCustomerExtension.Valid());
        var result = await _sut.PushInvoice(CachedCustomerExtension.Valid(), invoiceStub, 1);
        Assert.NotNull(result);
        Assert.True(result.DraftInvoiceNumber > 0);

        var draftInvoice = await _sut.GetDraftInvoice(result.DraftInvoiceNumber);
        Assert.NotNull(draftInvoice);
        Assert.Equal(result.DraftInvoiceNumber, draftInvoice.DraftInvoiceNumber);

        // Deletes draft invoice.
        var status = await _sut.DeleteInvoice(result.DraftInvoiceNumber);
        Assert.True(status);

        // Verify draft invoice is deleted.
        var emptyInvoice = await _sut.GetDraftInvoice(result.DraftInvoiceNumber);
        Assert.NotNull(emptyInvoice);
        Assert.True(emptyInvoice.DraftInvoiceNumber < 0);
    }

    [Fact]
    public async Task PushInvoice_When_Invoice_With_Invalid_PaymentTerm_Handle_Error()
    {
        Invoice invalidInvoice = InvoiceStubExtension.Valid(CachedCustomerExtension.Valid()).Invalid_PaymentTerm();
        var result = await _sut.PushInvoice(CachedCustomerExtension.Valid(), invalidInvoice, 1);
        Assert.NotNull(result);
        Assert.True(result.DraftInvoiceNumber == -1);
    }
}
