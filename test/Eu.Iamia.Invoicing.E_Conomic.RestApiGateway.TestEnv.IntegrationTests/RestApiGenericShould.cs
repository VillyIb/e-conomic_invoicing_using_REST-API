﻿using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using System.Net;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.TestEnv.IntegrationTests;

[NCrunch.Framework.Category("Integration")]
public class RestApiGenericShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiGenericShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }

    [Fact]
    public async Task GetAsyncFail()
    {
        const string reference = nameof(GetAsyncFail);

        const string requestUri = "https://restapi.e-conomic.com/invoices/drafts/9999";

        try
        {
            await ((RestApiService)_sut).GetAsync(requestUri, reference, _cts.Token);
            Assert.Fail("unexpected");
        }
        catch (HttpRequestException ex)
        {
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }
    }

    [Fact]
    public async Task PostAsyncFail()
    {
        const string reference = nameof(PostAsyncFail);

        const string requestUri = $"https://restapi.e-conomic.com//invoices/drafts";

        try
        {
            await ((RestApiService)_sut).PostAsync(requestUri, new StringContent(""), reference, _cts.Token);
            Assert.Fail("unexpected");
        }
        catch (HttpRequestException ex)
        {
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, ex.StatusCode);
        }
    }

    [Fact]
    public async Task DeleteAsyncFail()
    {
        const string reference = nameof(DeleteAsyncFail);

        const string requestUri = $"https://restapi.e-conomic.com/invoices/drafts/99999999";

        try
        {
            await ((RestApiService)_sut).DeleteAsync(requestUri, reference, _cts.Token);
            Assert.Fail("unexpected");
        }
        catch (HttpRequestException ex)
        {
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }
    }

}
