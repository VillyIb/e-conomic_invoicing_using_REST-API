

namespace Eu.Iamia.Reporting.Contract;
public interface ICustomerReport : IDisposable
{
    void Setup(ICustomer customer);

    ICustomerReport Create(DateTime timestamp);

    ICustomerReport Info(string reference, string message);

    ICustomerReport Error(string reference, string message);
}
