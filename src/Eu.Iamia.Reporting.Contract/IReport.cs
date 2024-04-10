namespace Eu.Iamia.Reporting.Contract;
public interface IReport : IDisposable
{
    void Setup()
    IReport Create(DateTime timestamp);

    IReport Info(string reference, string message);

    IReport Error(string reference, string message);
}
