using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class GatewayInvoiceShould
{
    private readonly GatewayBase _sut;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    public GatewayInvoiceShould()
    {
        using var setup = new Setup();
        _sut = (GatewayBase)setup.GetService<IEconomicGateway>();
    }

    [Fact]
    public async Task PushInvoice_When_ValidInvoice_DraftInvoice_IsCreated()
    {
        // Notice Creates and deletes a real draft-invoice in e-conomic

        Invoice invoiceStub = InvoiceStubExtension.Valid(CachedCustomerExtension.Valid());
        var result = await _sut.PushInvoice(CachedCustomerExtension.Valid(), invoiceStub, 1, _cts.Token);
        Assert.NotNull(result);
        Assert.True(result.DraftInvoiceNumber > 0);

        var draftInvoice = await _sut.GetDraftInvoice(result.DraftInvoiceNumber);
        Assert.NotNull(draftInvoice);
        Assert.Equal(result.DraftInvoiceNumber, draftInvoice.DraftInvoiceNumber);

        // Deletes draft invoice.
        var status = await _sut.DeleteDraftInvoice(result.DraftInvoiceNumber);
        Assert.True(status);

        // Verify draft invoice is deleted.
        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.GetDraftInvoice(result.DraftInvoiceNumber));
    }

    [Fact]
    public async Task PushInvoice_When_Invoice_With_Invalid_PaymentTerm_Handle_Error()
    {
        Invoice invalidInvoice = InvoiceStubExtension.Valid(CachedCustomerExtension.Valid()).Invalid_PaymentTerm();
        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.PushInvoice(CachedCustomerExtension.Valid(), invalidInvoice, 1, _cts.Token));
    }

    [Theory]
    [InlineData(0, 20)]
    public async Task GetBookedInvoices(int page, int pageSize)
    {
        var x = await _sut.GetBookedInvoice(page, pageSize, _cts.Token);
        Assert.NotNull(x);
    }

    [Fact]
    public async Task GetBookedInvoices2()
    {
        const int pageSize = 140;
        int page = 0;

        var Invoices = new List<InvoiceX>();

        while (true)
        {
            var x = await _sut.GetBookedInvoice(page++, pageSize, _cts.Token);

            if (!x.collection.Any()) break;

            foreach (var invoice in x.collection)
            {
                Invoices.Add(new InvoiceX { Customer = invoice.customer.customerNumber, Amount = invoice.grossAmount });
            }
        }
        
        var ordered = Invoices.OrderBy(entry => entry.Customer).ThenBy(entry => entry.Amount).ToList();

        var duplicates = new List<InvoiceX>();

        var initial = ordered.First();
        foreach (var inv in ordered.Skip(1))
        {
            if (initial.Customer == inv.Customer)
            {
                duplicates.Add(initial);
                duplicates.Add(inv);
            }
            initial = inv;
        }

    }

}

public class InvoiceX
{
    public int Customer { get; set; }

    public double Amount { get; set; }

    public override string ToString()
    {
        return $"{Customer} {Amount:00000.00}";
    }
}
