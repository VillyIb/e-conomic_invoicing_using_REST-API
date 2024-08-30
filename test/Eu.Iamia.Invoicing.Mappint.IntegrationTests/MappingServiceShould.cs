using System.Diagnostics;
using Eu.Iamia.Invoicing.Application.Contract.DTO;

namespace Eu.Iamia.Invoicing.Mapping.IntegrationTests;

public class MappingServiceShould
{
    private IMappingService _sut;

    private CancellationTokenSource _cts;

    public MappingServiceShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IMappingService>();
    }

    [Fact]
    public async Task LoadCustomerCache_OK()
    {
        var customerGroupsToAccept = new List<int> { 1, 2, 3 };
        var customerCount = await _sut.LoadCustomerCache(customerGroupsToAccept);
        Assert.True(customerCount > 1);
    }

    [Fact]
    public async Task LoadProductCache_OK()
    {
        var customerCount = await _sut.LoadProductCache();
        Assert.True(customerCount > 1);
    }

    [Fact]
    public async Task LoadPaymentTermCache_OK()
    {
        var paymentTermCount = await _sut.LoadPaymentTermCache();
        Assert.True(paymentTermCount > 1 );
    }

    [Fact]
    public async Task PushInvoice_OK()
    {
        const int sourceFileLineNumber = 999;

        var invoiceLines = new List<InvoiceLineDto>
        {
            new InvoiceLineDto
            {
                Description = "TestLine",
                SourceFileLineNumber = sourceFileLineNumber,
                ProductNumber = "99999",
                Quantity = 1.0,
                UnitNetPrice = 1.0,
                UnitNumber = 2,
                UnitText = "stk"
            }
        };

        var invoiceDto = new InvoiceDto
        {
            CustomerNumber = 99999,
            InvoiceDate = DateTime.Today,
            InvoiceLines = invoiceLines,
            PaymentTerm = 1,
            SourceFileLineNumber = sourceFileLineNumber,
            Text1 = "xx"
        };
        var layoutNumber = 21;

        List<int> customerGroupsToAccept = [1,2];
        await _sut.LoadCustomerCache(customerGroupsToAccept);
        await _sut.LoadProductCache();
        try
        {
            var x = await _sut.PushInvoice(invoiceDto, layoutNumber, sourceFileLineNumber, _cts.Token);
        }
        catch (HttpRequestException ex)
        {
            var msg = ex.Message;
            Debugger.Break();
        }
    }
}