using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Eu.Iamia.Reporting;

public class Report : IReport
{
    private readonly SettingsForReporting _settings;

    public static string JsonPrettify(string json)
    {
        try
        {
            using var jDoc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return json;
        }
    }

    public Report(IOptions<SettingsForReporting> settings)
    {
        _settings = settings.Value;
    }

    internal Report(SettingsForReporting settings)
    {
        _settings = settings;
    }

    private StreamWriter? _report;

    protected FileInfo? ReportFile;

    private bool HasErrors { get; set; } 


    public IReport Create(DateTime timestamp)
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

    public IReport Info(string reference, string message)
    {
        var p2 = JsonPrettify(message);

        EnsureOpenReport().Write($"{Environment.NewLine}Info: {reference}{Environment.NewLine}{p2}{Environment.NewLine}");
        return this;
    }

    public IReport Error(string reference, string message)
    {
        var p2 = JsonPrettify(message);

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
