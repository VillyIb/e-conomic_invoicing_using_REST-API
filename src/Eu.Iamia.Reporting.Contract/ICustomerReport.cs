

namespace Eu.Iamia.Reporting.Contract;
public interface ICustomerReport : IDisposable
{
    ICustomerReport SetCustomer(ICustomer customer);

    //ICustomerReport SetTime(DateTime timestamp);

    ICustomerReport Info(string reference, string message);

    ICustomerReport Error(string reference, string message);

    void Close();
}
