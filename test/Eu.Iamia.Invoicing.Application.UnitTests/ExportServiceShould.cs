using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Reporting.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            var x = await _sut.ExportBookedInvoices(dateInterval, cts.Token);
        }
        catch (Exception ex)
        {
            var _ = ex.ToString();
            Debugger.Break();
        }
    }


}
