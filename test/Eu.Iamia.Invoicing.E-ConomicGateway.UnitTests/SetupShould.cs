using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

[NCrunch.Framework.Category("Unit")]
public class SetupShould : IDisposable
{
    private readonly Setup _setup;

    public SetupShould()
    {
        _setup = new Setup();
    }

    [Fact]
    public void Instantiate_JsonSerializerFacade()
    {
        Assert.NotNull(_setup.GetService<IJsonSerializerFacade>());
    }

    [Fact]
    public void Instantiate_SerializerCustomersHandle()
    {
        Assert.NotNull(_setup.GetService<ISerializerCustomersHandle>());
    }

    [Fact]
    public void Instantiate_SerializerDraftInvoice()
    {
        Assert.NotNull(_setup.GetService<ISerializerDraftInvoice>());
    }

    [Fact]
    public void Instantiate_SerializerProductsHandle()
    {
        Assert.NotNull(_setup.GetService<ISerializerProductsHandle>());
    }

    [Fact]
    public void Instantiate_SerializerDeletedInvoices()
    {
        Assert.NotNull(_setup.GetService<ISerializerDeletedInvoices>());
    }

    [Fact]
    public void Instantiate_CustomerReport()
    {
        Assert.NotNull(_setup.GetService<ICustomerReport>());
    }

    [Fact]
    public void Instantiate_EconomicGateway()
    {
        Assert.NotNull(_setup.GetService<IEconomicGateway>());
    }

    public void Dispose()
    {
        _setup.Dispose();
    }
}
