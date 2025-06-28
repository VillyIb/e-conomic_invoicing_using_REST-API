using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Refit;
using Eu.Iamia.Utils;
using System.Net;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests;

[NCrunch.Framework.Category("Integration")]

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

    // this should be a unit test validating the generated query.
    //[Theory]
    //[InlineData(1, 66, "2024-01-01", "2024-12-31")]
    //public async Task ReadBookedInvoices(int page, int pageSize, string from, string to)
    //{
    //    var dateRange = Interval<DateTime>.Create(DateTime.Parse(from), DateTime.Parse(to));
    //    var bookedInvoicesHandle = await ((GatewayV2TestVariant)_sut).ReadBookedInvoices(page, pageSize, dateRange, _cts.Token);

    //    Assert.NotNull(bookedInvoicesHandle);
    //    Assert.True(bookedInvoicesHandle.Invoices.Any());
    //    Assert.Equal(pageSize, bookedInvoicesHandle.Invoices.Length);
    //}

    [Theory]
    [InlineData(56, 2024072)]
    public async Task ReadBookedInvoice(int customerNumber, int invoiceNumber)
    {
        var bookedInvoice = await _sut.ReadBookedInvoice(invoiceNumber);
        Assert.NotNull(bookedInvoice);
        Assert.Equal(customerNumber, bookedInvoice.customer.customerNumber);
    }

    [Theory]
    [InlineData(0, 20)] // test environment only contains 5 customers no. 1..5
    [InlineData(1, 20)]
    public async Task ReadCustomers(int page, int pageSize)
    {
        var customersHandle = await _sut.ReadCustomers(page, pageSize);

        Assert.NotNull(customersHandle);
        Assert.True(customersHandle.Customers.Any());
    }

    [Theory]
    //[Theory(Skip = "no draft invoices")]
    [InlineData(0, 20, true)] // Assume some draft invoices exist in the test environment.
    [InlineData(1, 20, false)] // Assume no draft invoices exist in the test environment.
    public async Task ReadDraftInvoices(int page, int pageSize, bool expectInvoices)
    {
        var draftInvoicesHandle = await _sut.ReadDraftInvoices(page, pageSize);

        Assert.NotNull(draftInvoicesHandle);
        if (expectInvoices)
        {
            Assert.True(draftInvoicesHandle.Invoices.Any());
        }
        else
        {
            Assert.False(draftInvoicesHandle.Invoices.Any());
        }
    }

    [Theory]
    //[Theory(Skip = "unknown invoices")]
    [InlineData(516, false)] // Assume invoice 516 does NOT exist in the test environment.
    [InlineData(517, true)] // Assume invoice 517 exists in the test environment.
    public async Task GetDraftInvoice(int invoiceNo, bool expectFound)
    {


        if (expectFound)
        {
            var draftInvoice = await _sut.GetDraftInvoice(invoiceNo);

            Assert.NotNull(draftInvoice);
            Assert.Equal(invoiceNo, draftInvoice.draftInvoiceNumber);
        }
        else
        {
            ApiException ex = await Assert.ThrowsAsync<ApiException>(async () => await _sut.GetDraftInvoice(invoiceNo));
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }
    }

}
