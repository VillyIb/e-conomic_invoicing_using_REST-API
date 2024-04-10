namespace Eu.Iamia.Reporting.Contract;

public interface ICustomer
{
    int CustomerNumber { get; }

    string? Name { get; }
}