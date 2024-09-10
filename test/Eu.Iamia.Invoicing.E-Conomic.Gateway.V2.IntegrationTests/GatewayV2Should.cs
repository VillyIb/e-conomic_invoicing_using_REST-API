using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests;
public class GatewayV2Should
{
    private readonly IEconomicGatewayV2 _sut;
    private readonly CancellationTokenSource _cts;

    public GatewayV2Should()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IEconomicGatewayV2>();
    }

    [Fact]
    public async Task LoadPaymentTermsCache_OK()
    {
        var cacheCount = await _sut.LoadPaymentTermsCache();
        Assert.True(cacheCount > 1 );
    }

    [Theory]
    [InlineData("Lb. md. 14 dage", 1)]
    [InlineData("14 dage", 2)]
    [InlineData("30 dage", 3)]
    [InlineData("99999", 4)]
    [InlineData("8 dage", 5)]
    [InlineData(null, 99)]
    public async Task GetPaymentTerm(string? expectedName, int paymentTermNumber)
    {
        await _sut.LoadPaymentTermsCache();
        var actual = _sut.GetPaymentTerm(paymentTermNumber);
        if (expectedName is null)
        {
            Assert.Null(actual);
        }
        else
        {
            Assert.NotNull(actual);
            Assert.Equal(expectedName, actual.name);
        }
    }

    [Theory]
    [InlineData(1,66, "2024-01-01", "2024-12-31")]
    public async Task ReadBookedInvoices(int page, int pageSize, string from, string to)
    {
        var dateRange = Interval<DateTime>.Create(DateTime.Parse(from), DateTime.Parse(to));
        var bookedInvoicesHandle = await ((GatewayV2TestVariant)_sut).ReadBookedInvoices(page, pageSize, dateRange, _cts.Token);

        Assert.NotNull(bookedInvoicesHandle);
        Assert.True(bookedInvoicesHandle.Invoices.Any());
        Assert.Equal(pageSize, bookedInvoicesHandle.Invoices.Length );
    }

    [Theory]
    [InlineData(56,2024072)]
    public async Task ReadBookedInvoice(int customerNumber, int invoiceNumber)
    {
        var bookedInvoice = await _sut.ReadBookedInvoice(invoiceNumber);
        Assert.NotNull(bookedInvoice);
        Assert.Equal(customerNumber,bookedInvoice.customer.customerNumber);
    }
    
}
