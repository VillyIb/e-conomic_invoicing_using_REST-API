using System.Diagnostics.CodeAnalysis;
using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Reporting;

public abstract class ReportBase
{

}

public class CustomerCustomerReport : ReportBase, ICustomerReport
{
    private readonly SettingsForReporting _settings;

    [ExcludeFromCodeCoverage]
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

    private ICustomer? _customer;

    public void Setup(ICustomer customer)
    {
        _customer = customer;
        //throw new NotImplementedException();
    }

    internal string GetFilename(bool exception)
    {
        if (_customer is null) { throw new ApplicationException($"{nameof(_customer)} is null"); }

        var namePart = (_customer.Name is not null)
            ? _customer.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            : new[] { "", "" }
        ;

        var number = _customer.CustomerNumber.ToString().TrimNumberToLength(_settings.CustomerNumberLength);
        var firstname = namePart.First().TrimToLength(_settings.CustomerNameLength, 'f');
        var lastname = namePart.Last().TrimToLength(_settings.CustomerSurnameLength, 'l');
        var customerPart = $"{number}_{firstname}-{lastname}";
        var timePart = _timeStamp.ToString(_settings.TimePartFormat);
        var statusPart = exception ? "E" : "I";
        var contentPart = _settings.Filename;

        return Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_{statusPart}_{contentPart}");
    }

    private DateTime _timeStamp;

    public ICustomerReport Create(DateTime timestamp)
    {
        _timeStamp = timestamp;
        Close();
        HasErrors = false;
        var timestampString = timestamp.ToString(_settings.TimePartFormat);
        var filename = GetFilename(false);
        ReportFile = new FileInfo(filename);
        var fs = ReportFile.OpenWrite();
        _report = new StreamWriter(fs);
        return this;
    }

    private StreamWriter EnsureOpenReport()
    {
        if (_report is null)
        {
            throw new ApplicationException($"{nameof(_report)} is null");
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

        if (HasErrors)
        {
            var fs = new FileInfo(GetFilename(false));
            fs.MoveTo(GetFilename(true));
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
