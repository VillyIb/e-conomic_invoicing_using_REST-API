﻿using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.TestEnv.IntegrationTests;

[NCrunch.Framework.Category("Integration")]

public class RestApiPaymentTermShould
{
    private readonly IRestApiGateway _sut;
    private readonly CancellationTokenSource _cts;

    public RestApiPaymentTermShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IRestApiGateway>();
    }
    
    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 20)]
    public async Task GetPaymentTerms_OK(int page, int pageSize)
    {
        var stream = await _sut.GetPaymentTerms(page, pageSize, _cts.Token);

        var reade = new StreamReader(stream);
        var json = await reade.ReadToEndAsync(_cts.Token);

        Assert.NotNull(json);
        Assert.False(string.IsNullOrEmpty(json));
    }
}
