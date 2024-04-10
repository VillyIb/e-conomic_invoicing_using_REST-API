using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Reporting;

public abstract class ReportBase
{

}

public class CustomerCustomerReport : ReportBase, ICustomerReport
{
    private readonly SettingsForReporting _settings;

    public CustomerCustomerReport(IOptions<SettingsForReporting> settings)
    {
        _settings = settings.Value;
    }

    internal CustomerCustomerReport(SettingsForReporting settings)
    {
        _settings = settings;
    }

    private StreamWriter? _report;

    protected FileInfo? ReportFile;

    private bool HasErrors { get; set; }
    

    public void Setup(ICustomer customer)
    {
        throw new NotImplementedException();
    }

    public ICustomerReport Create(DateTime timestamp)
    {
        Close();
        HasErrors = false;
        var timestampString = timestamp.ToString(_settings.FilNameFormat);
        var filename = Path.Combine(_settings.OutputDirectory,string.Format(_settings.Filename, timestampString));
        ReportFile = new FileInfo(filename);
        var fs = ReportFile.OpenWrite();
        _report = new StreamWriter(fs);
        return this;
    }

    private StreamWriter EnsureOpenReport()
    {
        if (_report is null)
        {
            Create(DateTime.Now);
        }

        return _report!;
    }

    public ICustomerReport Info(string reference, string message)
    {
        var p2 = message.JsonPrettify();

        EnsureOpenReport().Write($"{Environment.NewLine}Info: {reference}{Environment.NewLine}{p2}{Environment.NewLine}");
        return this;
    }

    public ICustomerReport Error(string reference, string message)
    {
        var p2 = message.JsonPrettify();

        EnsureOpenReport().Write($"{Environment.NewLine}Error: {reference}{Environment.NewLine}{p2}{Environment.NewLine}");
        HasErrors = true;
        return this;
    }

    public void Close()
    {
        if (_report == null) return;
        
        _report.Flush();
        _report.Close();

        if (!HasErrors && _settings.DiscardNonErrors)
        {
            ReportFile!.Delete();
        }

        _report = null;
    }

    public void Dispose()
    {
        Close();
    }
}

public class AggregateReport : ReportBase
{

}
