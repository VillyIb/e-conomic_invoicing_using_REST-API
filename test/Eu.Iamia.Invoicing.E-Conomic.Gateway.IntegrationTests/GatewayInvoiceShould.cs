using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    [InlineData(17, 0, 17, "2024-01-01", "2024-01-31")]
    [InlineData(17, 1, 17, "2024-01-01", "2024-01-31")]
    [InlineData(17, 7, 17, "2024-01-01", "2024-01-31")]
    [InlineData(04, 8, 17, "2024-01-01", "2024-01-31")] 
    public async Task GetBookedInvoices(int expectedCount, int page, int pageSize, string fromDate, string toDate)
    {
        var fd = DateTime.Parse(fromDate);
        var td = DateTime.Parse(toDate);
        var dateRange = Interval<DateTime>.Create(fd, td);
        var x = await _sut.GetBookedInvoice(
            page,
            pageSize,
            dateRange,
            _cts.Token
        );
        Assert.NotNull(x);
        Assert.Equal(expectedCount,x.collection.Length);
    }

    [Fact]
    public async Task GetBookedInvoices2()
    {
        const int pageSize = 140;
        int page = 0;
        var fd = DateTime.Parse("2024-01-01");
        var td = DateTime.Parse("2024-01-31");
        var dateRange = Interval<DateTime>.Create(fd, td);

        var invoices = new List<InvoiceX>();

        while (true)
        {
            var x = await _sut.GetBookedInvoice(page++, pageSize, dateRange, _cts.Token);

            if (!x.collection.Any()) break;

            foreach (var invoice in x.collection)
            {
                invoices.Add(new InvoiceX { Customer = invoice.customer.customerNumber, Amount = invoice.grossAmount });
            }
        }

        Assert.Equal(140, invoices.Count);
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
