using Eu.Iamia.Reporting.Configuration;

namespace Eu.Iamia.Reporting.IntegrationTests;
public class CustomerReportForTesting : CustomerCustomerReport
{
    public CustomerReportForTesting(SettingsForReporting settings) : base(settings)
    { }
   
    public bool Exists(string filename)
    {
        var t1 = new FileInfo(filename);
        t1.Refresh();
        var t2 = t1.Exists;

        return t2;
    }

    public string GetContent()
    {
        if (ReportFile is null || !ReportFile.Exists) return string.Empty;
        using var sr = new StreamReader(ReportFile.OpenRead());
        return sr.ReadToEnd();
    }

    internal int? GetCustomerNumber()
    {
        return CustomerNumber;
    }

    internal string? GetCustomerName()
    {
        return CustomerName;
    }

    internal DateTime? GetTimeStamp()
    {
        return _timeStamp;
    }

    internal void DeleteFile(string filename)
    {
        var fi = new FileInfo(filename);
        if (fi.Exists)
        {
            fi.Delete();

        }
    }
}
