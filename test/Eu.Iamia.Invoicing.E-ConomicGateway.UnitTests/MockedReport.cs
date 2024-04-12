using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

internal class MockedReport : ICustomerReport
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public ICustomerReport SetCustomer(ICustomer customer)
    {
        return this;
    }

    public ICustomerReport Info(string reference, string message)
    {
        return this;
    }

    public ICustomerReport Error(string reference, string message)
    {
        return this;
    }

    public void Close()
    {
        
    }
}
