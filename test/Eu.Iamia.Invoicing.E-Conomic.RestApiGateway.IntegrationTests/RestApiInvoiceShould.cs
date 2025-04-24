using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Utils;
using System.Text;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiInvoiceShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiInvoiceShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 20)]
    public async Task GetDraftInvoices(int page, int pageSize)
    {
        var stream = await _sut.GetDraftInvoices(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    //[Theory(Skip = "Requires draft invoice to exist")]
    [Theory]
    [InlineData(454)]
    public async Task GetDraftInvoice(int invoiceNo)
    {
        var stream = await _sut.GetDraftInvoice(invoiceNo, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetBookedInvoices(int page, int pageSize)
    {
        var from = DateTime.Parse("2024-01-01");
        var to = DateTime.Parse("2024-01-31");
        var dateRange = Interval<DateTime>.Create(from, to);
        var stream = await _sut.GetBookedInvoices(page, pageSize, dateRange, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Theory]
    [InlineData(16)]
    [InlineData(18)]
    public async Task GetBookedInvoice(int invoiceNo)
    {
        var stream = await _sut.GetBookedInvoice(invoiceNo, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    //[Fact(Skip = "Notice! Leaves draft invoice")]
    [Fact]
    public async Task PostDraftInvoice()
    {
        // Notice leaves draft invoice.
        // use: Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests.Stubs.InvoiceStub.GetInvoiceJson() to create valid json.
        var json = "{\"date\":\"2024-07-26\",\"currency\":\"DKK\",\"exchangeRate\":100,\"paymentTerms\":{\"paymentTermsNumber\":1,\"paymentTermsType\":\"invoiceMonth\"},\"customer\":{\"customerNumber\":99999},\"recipient\":{\"name\":\"CustomerName\",\"address\":\"CustomerAddress\",\"zip\":\"9999\",\"city\":\"CustomerCity\",\"vatZone\":{\"name\":\"Domestic\",\"vatZoneNumber\":1,\"enabledForCustomer\":true,\"enabledForSupplier\":true}},\"references\":{},\"layout\":{\"layoutNumber\":21},\"lines\":[{\"lineNumber\":1,\"sortKey\":1,\"unit\":{\"unitNumber\":1,\"name\":\"mdr\"},\"product\":{\"productNumber\":\"99999\"},\"quantity\":1,\"unitNetPrice\":1,\"discountPercentage\":0,\"unitCostPrice\":0,\"totalNetAmount\":1,\"marginInBaseCurrency\":0,\"marginPercentage\":100,\"description\":\"Description line 1\"},{\"lineNumber\":2,\"sortKey\":2,\"unit\":{\"unitNumber\":1,\"name\":\"mdr\"},\"product\":{\"productNumber\":\"99999\"},\"quantity\":1,\"unitNetPrice\":1,\"discountPercentage\":0,\"unitCostPrice\":0,\"totalNetAmount\":1,\"marginInBaseCurrency\":0,\"marginPercentage\":100,\"description\":\"Description line 2\"},{\"lineNumber\":3,\"sortKey\":3,\"unit\":{\"unitNumber\":1,\"name\":\"mdr\"},\"product\":{\"productNumber\":\"99999\"},\"quantity\":1,\"unitNetPrice\":1,\"discountPercentage\":0,\"unitCostPrice\":0,\"totalNetAmount\":1,\"marginInBaseCurrency\":0,\"marginPercentage\":100,\"description\":\"Description line 3\"}],\"notes\":{\"heading\":\"#99999 CustomerName\",\"textLine1\":\"InvoiceText1\"}}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var stream = await _sut.PostDraftInvoice(content, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }
}