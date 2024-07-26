using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiInvoiceShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiInvoiceShould()
    {
        _cts  = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetDraftInvoicesPaged(int page, int pageSize)
    {
        var stream = await _sut.GetDraftInvoices(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var text = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(text);
        Assert.False(string.IsNullOrEmpty(text));
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetX(int page, int pageSize)
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
}