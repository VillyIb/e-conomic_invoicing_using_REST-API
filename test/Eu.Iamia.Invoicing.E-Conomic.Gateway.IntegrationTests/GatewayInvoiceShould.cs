using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class GatewayInvoiceShould
{
    private readonly GatewayBase _sut;

    public GatewayInvoiceShould()
    {
        using var setup = new Setup();

        _sut = (GatewayBase)setup.GetService<IEconomicGateway>();
    }

    //[Fact]
    //public async Task GivenFaultyInputInvoice_When_PushInvoice_HandleError()
    //{
    //    IInputInvoice invalidInvoice = MockedInputInvoiceExtension.Valid;
    //    var result = await sut.PushInvoice(invalidInvoice, 0);
    //}

    [Fact]
    public async Task PushInvoice_When_InvalidInvoice_Handle_XXXX()
    {
        Invoice invalidInvoice = MockedInvoiceExtensions.Valid(MockedCustomer.Valid());
        var result = await _sut.PushInvoice(MockedCustomer.Valid(), invalidInvoice, 1);
        Assert.NotNull(result);
        Assert.True(result.DraftInvoiceNumber > 0);
    }

    [Fact]
    public async Task PushInvoice_When_Invoice_With_Invalid_PaymentTerm_Handle_Error()
    {
        Invoice invalidInvoice = MockedInvoiceExtensions.Valid(MockedCustomer.Valid()).Invalid_PaymentTerm();
        var result = await _sut.PushInvoice(MockedCustomer.Valid(), invalidInvoice, 1);
    }
}
