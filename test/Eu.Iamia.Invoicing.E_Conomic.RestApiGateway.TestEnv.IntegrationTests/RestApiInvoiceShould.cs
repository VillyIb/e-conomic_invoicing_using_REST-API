﻿using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.TestEnv.IntegrationTests;

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
    [InlineData(0, 20)] // test environment contains 7 draft invoices # 4..10, 13 abd 15.
    [InlineData(1, 20)]
    public async Task GetDraftInvoices(int page, int pageSize)
    {
        var stream = await _sut.GetDraftInvoices(page, pageSize, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }

    [Theory(Skip = "unknown invoices")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(13)]
    [InlineData(15)]
    public async Task GetDraftInvoice(int invoiceNo)
    {
        var stream = await _sut.GetDraftInvoice(invoiceNo, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }


    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetBookedInvoices(int page, int pageSize)
    {
        // Test environment contains 6 invoices date range 2022-06-02, 2022-06-03, 2022-06-07 with 2, 1 and 3 invoices, number 1..6
        var from = DateTime.Parse("2022-06-01");
        var to = DateTime.Parse("2022-06-07");
        var dateRange = Interval<DateTime>.Create(from, to);
        var stream = await _sut.GetBookedInvoices(page, pageSize, dateRange, _cts.Token);
        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetBookedInvoice(int invoiceNo)
    {
        var stream = await _sut.GetBookedInvoice(invoiceNo, _cts.Token);

        Assert.NotNull(stream);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }

    [Theory]
    [InlineData(7)]
    public async Task GetBookedInvoiceFail(int invoiceNo)
    {
        try
        {
            await _sut.GetBookedInvoice(invoiceNo, _cts.Token);
        }
        catch (HttpRequestException ex)
        {
            var jsonError = ex.Message;
            Assert.Equal(HttpRequestError.Unknown, ex.HttpRequestError);
        }
    }

}