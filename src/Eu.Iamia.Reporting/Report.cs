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

    protected int? CustomerNumber { get; set; } = null;

    protected string? CustomerName { get; set; } = null;

    internal bool IsOpenForWriting => _report?.BaseStream is not null;

    public ICustomerReport SetCustomer(ICustomer customer)
    {
        if (IsOpenForWriting)
        {
            return this;
        }

        if (!IsSameCustomer(customer))
        {
            _timeStamp = null;
            HasErrors = false;
        }

        CustomerNumber = customer.CustomerNumber;
        CustomerName = customer.Name;

        return this;
    }

    private bool IsSameCustomer(ICustomer customer)
    {
        return customer.CustomerNumber.Equals(CustomerNumber);
    }

    //public ICustomerReport SetTime(DateTime timestamp)
    //{
    //    if (ReportFile is not null && ReportFile.Exists) { return this; }
    //    _timeStamp = timestamp;
    //    return this;
    //}

    internal string GetFilename(bool hasErrors)
    {
        if (CustomerNumber is null) { throw new ApplicationException($"{nameof(CustomerNumber)} is null"); }

        var namePart = (CustomerName is not null)
            ? CustomerName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            : new[] { "", "" }
        ;

        var number = CustomerNumber.ToString().TrimNumberToLength(_settings.CustomerNumberLength);
        var firstname = namePart.First().TrimToLength(_settings.CustomerNameLength, 'f');
        var lastname = namePart.Last().TrimToLength(_settings.CustomerSurnameLength, 'l');
        var customerPart = $"{number}_{firstname}-{lastname}";
        var timePart = _timeStamp!.Value.ToString(_settings.TimePartFormat);
        var statusPart = hasErrors ? "E" : "I";
        var contentPart = _settings.Filename;

        return Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_{statusPart}_{contentPart}");
    }

    protected DateTime? _timeStamp = null;

    internal void Create()
    {
        _timeStamp ??= DateTime.Now;
        Close();
        var filename = GetFilename(HasErrors);
        ReportFile = new FileInfo(filename);
        FileStream fs;
        if (ReportFile.Exists)
        {
            fs = ReportFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Position = fs.Length;
        }
        else
        {
            fs = ReportFile.OpenWrite();
        }
        _report = new StreamWriter(fs);
    }

    private StreamWriter EnsureOpenReport()
    {
        if (_report is null)
        {
            Create();
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
        _report = null;

        if (!HasErrors && _settings.DiscardNonErrorLogfiles)
        {
            ReportFile!.Delete();
            return;
        }

        if (!HasErrors)
            return;

        var fs = new FileInfo(GetFilename(false));
        if (fs.Exists)
        {
            fs.MoveTo(GetFilename(true));
            ReportFile = new FileInfo(fs.FullName);
        }
    }

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        Close();
    }
}

public class AggregateReport : ReportBase
{

}
