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
    public CustomerCustomerReport(IOptions<SettingsForReporting> settings) : this(settings.Value)
    { }

    internal CustomerCustomerReport(SettingsForReporting settings)
    {
        _settings = settings;
        ReportStateStrategy = new ReportStateStrategy(ReportState.Info);
    }

    private StreamWriter? _report;

    protected FileInfo? ReportFile;

    private ReportState ReportState
    {
        get => ReportStateStrategy.ReportState;
        set
        {
            if (ReportStateStrategy.Locked) return;
            ReportStateStrategy = new ReportStateStrategy(value);
        }
    }

    internal ReportStateStrategy ReportStateStrategy { get; private set; }

    protected int? CustomerNumber { get; set; }

    protected string? CustomerName { get; set; }

    internal bool IsOpenForWriting => _report?.BaseStream is not null;

    public ICustomerReport SetCustomer(ICustomer customer)
    {
        if (IsOpenForWriting)
        {
            return this;
        }

        if (!IsSameCustomer(customer))
        {
            TimeStamp = null;
            ReportStateStrategy = new ReportStateStrategy(ReportState.Info);
            ReportState = ReportState.Info;
        }

        CustomerNumber = customer.CustomerNumber;
        CustomerName = customer.Name;

        return this;
    }

    private bool IsSameCustomer(ICustomer customer)
    {
        return
            CustomerNumber == customer.CustomerNumber 
            &&
            CustomerName == customer.Name
        ;
    }

    internal string GetFilename(ReportState reportState)
    {
        if (CustomerNumber is null) { throw new ApplicationException($"{nameof(CustomerNumber)} is not provided"); }

        var namePart = (CustomerName is not null)
            ? CustomerName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            : new[] { "", "" }
        ;

        var number = CustomerNumber.ToString().TrimNumberToLength(_settings.CustomerNumberLength);
        var firstname = namePart.First().TrimToLength(_settings.CustomerNameLength, 'f');
        var lastname = namePart.Last().TrimToLength(_settings.CustomerSurnameLength, 'l');
        var customerPart = $"{number}_{firstname}-{lastname}";
        var timePart = TimeStamp!.Value.ToString(_settings.TimePartFormat);
        var statusPart = new ReportStateStrategy(reportState).StatusPart;
        var contentPart = _settings.Filename;

        return Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_{statusPart}_{contentPart}");
    }

    protected DateTime? TimeStamp;

    internal void Create()
    {
        TimeStamp ??= DateTime.Now;
        Close();
        var filename = GetFilename(ReportState);
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

    public ICustomerReport Message(string reference, string message)
    {
        var p2 = message.JsonPrettify();

        EnsureOpenReport().Write($"{Environment.NewLine}Error: {reference}{Environment.NewLine}{p2}{Environment.NewLine}");
        ReportState = ReportState.Message;
        return this;
    }

    public ICustomerReport Error(string reference, string message)
    {
        var p2 = message.JsonPrettify();

        EnsureOpenReport().Write($"{Environment.NewLine}Error: {reference}{Environment.NewLine}{p2}{Environment.NewLine}");
        EnsureOpenReport().Flush();
        ReportState = ReportState.Error;
        return this;
    }

    public void Close()
    {
        if (_report == null) return;

        _report.Flush();
        _report.Close();
        _report = null;

        var reportStateStrategy = new ReportStateStrategy(ReportState);

        if (reportStateStrategy.PruneFileAfterClose && _settings.PruneLogfiles)
        {
            ReportFile!.Delete();
            return;
        }

        if (reportStateStrategy.PruneFileAfterClose)
            return;

        var fs = new FileInfo(GetFilename(ReportState.Info));
        if (fs.Exists)
        {
            fs.MoveTo(GetFilename(ReportState));
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
{ }
