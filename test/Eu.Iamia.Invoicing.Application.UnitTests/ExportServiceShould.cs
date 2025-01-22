using Eu.Iamia.Invoicing.Application.Configuration;
using System.Diagnostics;
using System.Reflection;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.Application.UnitTests;

[NCrunch.Framework.Category("Unit")]

public class ExportServiceShould
{
    private readonly IExportService _sut;
    private readonly CancellationTokenSource cts;

    public ExportServiceShould()
    {
        cts = new CancellationTokenSource(100000);

        var executingDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;

        using var setup = new Setup();
        var settingsForInvoicingApplication = setup.GetSetting<SettingsForInvoicingApplication>();
        _sut = setup.GetService<IExportService>();

        //_outputDirectory = new DirectoryInfo(setup.GetSetting<SettingsForReporting>().OutputDirectory);
        //CleanTestDataDirectory(_outputDirectory);
    }

    [Fact]
    public async Task ExportBookedInvoices()
    {
        var dateInterval = Interval<DateTime>.Create(DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"));
        try
        {
            var x = await _sut.ExportBookedInvoices(dateInterval, false, cts.Token);
        }
        catch (Exception ex)
        {
            var _ = ex.ToString();
            Debugger.Break();
            throw;
        }
    }
}
