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
            var expected = new FileInfo($"C:\\Development\\Logfiles\\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_BookedInvoices.csv");

            Assert.False(expected.Exists);

            var x = await _sut.ExportBookedInvoices(dateInterval, false, expected, cts.Token);

            expected.Refresh();
            Assert.True(expected.Exists);
            Assert.Equal(482, expected.Length);
            expected.Delete();
        }
        catch (Exception ex)
        {
            var _ = ex.ToString();
            Debugger.Break();
            throw;
        }
    }
}
