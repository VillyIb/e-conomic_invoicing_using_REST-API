using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Utils;
using Refit;
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

    [Theory]
    [InlineData(0, 20, true)]
    [InlineData(99, 20, false)]
    public async Task GetPaymentTerms(int page, int pageSize, bool expectData)
    {
        var stream = await _sut.GetPaymentTerms(page, pageSize);
        Assert.NotNull(stream);

        if (expectData)
        {
            Assert.True(stream.PaymentTerms.Any());
        }
        else
        {
            Assert.False(stream.PaymentTerms.Any());
        }
    }

    [Theory]
    [InlineData(000, 20, true)] // test environment only contains 5 customers no. 1..5
    [InlineData(999, 20, false)]
    public async Task GetCustomers(int page, int pageSize, bool expectCustomers)
    {
        var customersHandle = await _sut.GetCustomers(page, pageSize);

        Assert.NotNull(customersHandle);
        Assert.Equal(expectCustomers, customersHandle.Customers.Any());
    }

    [Theory]
    [InlineData(00, 20, true)] // test environment contains product 1..7
    [InlineData(99, 20, false)]
    public async Task GetProducts(int page, int pageSize, bool expectProducts)
    {
        var producsHandle = await _sut.GetProducts(page, pageSize);

        Assert.NotNull(producsHandle);
        Assert.Equal(expectProducts, producsHandle.Products.Any());
    }

    [Theory]
    //[Theory(Skip = "no draft invoices")]
    [InlineData(0, 20, true)] // Assume some draft invoices exist in the test environment.
    [InlineData(1, 20, false)] // Assume no draft invoices exist in the test environment.
    public async Task GetDraftInvoices(int page, int pageSize, bool expectInvoices)
    {
        var draftInvoicesHandle = await _sut.GetDraftInvoices(page, pageSize);

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

    // Post Draft Invoice

    [Theory]
    [InlineData(516, true)] // Assume invoice 516 does NOT exist in the test environment.
    public async Task PostDraftInvoice(int invoiceNo, bool expectSuccess)
    {
        var self = new Uri("https://restapi.e-conomic.com/customers/93/"); // Base URL for the API, adjust as necessary.
        //new Uri("//restapi.e-conomic.com/customers/93/").ToString()

        var draftInvoice = new Contract.DTO.Invoices.drafts.post.Invoice()
        {
            Customer = new Contract.DTO.Invoices.drafts.post.Customer(93,self.ToString() ),
            Date = $"{DateTime.Now:yyyy-MM-dd}", // Use current date in the format required by the API.
            Layout = new Contract.DTO.Invoices.drafts.post.Layout() { LayoutNumber = 21 }, // Assuming layout 1 exists.
            Lines = new List<Contract.DTO.Invoices.drafts.post.Line>
            {
                new Contract.DTO.Invoices.drafts.post.Line
                {
                    Description = "Test Line",
                    LineNumber = 1,
                    Product= new Contract.DTO.Invoices.drafts.post.Product(){ProductNumber = "1"}, // Assuming product 1 exists.
                    Quantity = 1.0,
                    SortKey = 1,
                    Unit = new Contract.DTO.Invoices.drafts.post.Unit("stk", 1),
                    UnitNetPrice = 100.00,
                    DiscountPercentage = 0.0,
                    MarginInBaseCurrency = 100.0,
                    MarginPercentage = 100.0,
                    TotalNetAmount = 100.0,
                    UnitCostPrice = 0.0
                }
            },
            PaymentTerms = new Contract.DTO.Invoices.drafts.post.PaymentTerms()
            {
                PaymentTermsNumber = 1,  // Assuming payment term 1 exists.
                PaymentTermsType = Contract.DTO.Invoices.drafts.post.PaymentTermsType.invoiceMonth
            },
            Recipient = new Contract.DTO.Invoices.drafts.post.Recipient()
            {
                Address = "Test Address",
                City = "Test City",
                Name = "Test Recipient",
                VatZone = new Contract.DTO.Invoices.drafts.post.VatZone()
                {
                    VatZoneNumber = 1 // Assuming VAT zone 1 exists.
                },
                Zip = "1234" // Example ZIP code.
            },
            Delivery = new Contract.DTO.Invoices.drafts.post.Delivery()
            {
                Address = "Test Delivery Address",
                City = "Test Delivery City",
                Country = "Test Delivery Name",
                DeliveryDate = $"{DateTime.Now:yyyy-MM-dd}", // Use current date in the format required by the API.
                Zip = "1234" // Example ZIP code.
            },
            References = new Contract.DTO.Invoices.drafts.post.References() { Other = string.Empty },
            Notes = new Contract.DTO.Invoices.drafts.post.Notes()
            {
                Heading = string.Empty, // Optional heading.
                TextLine1 = "Test Note Line 1",
                TextLine2 = string.Empty
            }
        };

        if (expectSuccess)
        {
            try
            {                 // Attempt to post the draft invoice.
                var createdDraftInvoice = await _sut.PostDraftInvoice(draftInvoice);
                Assert.NotNull(createdDraftInvoice);
                Assert.Equal(invoiceNo, createdDraftInvoice.DraftInvoiceNumber);


                Assert.NotNull(createdDraftInvoice);


            }
            catch (ApiException ex)
            {
                var content = $"Text: {Content.FromJson(ex.Content)}";
                Assert.True(false, $"Unexpected exception: {ex.Message}");
            }

        }
        else
        {
            ApiException ex = await Assert.ThrowsAsync<ApiException>(async () => await _sut.PostDraftInvoice(draftInvoice));
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }
    }

    [Theory]
    [InlineData(00, 20, "2024-01-01", "2024-12-31", true)]
    [InlineData(99, 20, "2024-01-01", "2024-12-31", false)]
    public async Task GetBookedInvoices(int page, int pageSize, string _dateFrom, string _DateTo, bool expectInvoices)
    {
        var dateFrom = DateTime.Parse(_dateFrom);
        var dateTo = DateTime.Parse(_DateTo);
        var interval = Interval<DateTime>.Create(dateFrom, dateTo);
        var bookedInvoicesHandle = await _sut.GetBookedInvoices(page, pageSize, interval);
        Assert.NotNull(bookedInvoicesHandle);
        Assert.Equal(expectInvoices, bookedInvoicesHandle.Invoices.Any());
    }

    [Theory]
    [InlineData(2024072, true)]
    [InlineData(9999999, false)]
    public async Task GetBookedInvoice(int invoiceNumber, bool expetFound)
    {
        if (expetFound)
        {
            var bookedInvoice = await _sut.GetBookedInvoice(invoiceNumber);
            Assert.NotNull(bookedInvoice);
            Assert.NotNull(bookedInvoice.customer);
            Assert.Equal(invoiceNumber, bookedInvoice.bookedInvoiceNumber);
        }
        else
        {
            ApiException ex = await Assert.ThrowsAsync<ApiException>(async () => await _sut.GetBookedInvoice(invoiceNumber));
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }
    }

}
