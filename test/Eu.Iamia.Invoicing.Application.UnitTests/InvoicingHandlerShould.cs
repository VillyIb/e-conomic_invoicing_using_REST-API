using System.Reflection;
using Eu.Iamia.Invoicing.Application.Configuration;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Reporting.Configuration;

namespace Eu.Iamia.Invoicing.Application.UnitTests;

[NCrunch.Framework.Category("Unit")]

public class InvoicingHandlerShould : IDisposable
{
    private readonly IInvoicingHandler _sut;
    private readonly DirectoryInfo _outputDirectory;

    private static void CleanTestDataDirectory(DirectoryInfo directory)
    {
        var files = directory.GetFiles("*_InvoiceReport.txt");
        if (files.Length > 2) return; // don't delete wild!
        foreach (var file in files)
        {
            file.Delete();
        }
    }

    private static int CountFiles(DirectoryInfo directory, string filter)
    {
        return directory.GetFiles(filter).Length;
    }

    public InvoicingHandlerShould()
    {
        var executingDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;

        using var setup = new Setup();
        var settingsForInvoicingApplication = setup.GetSetting<SettingsForInvoicingApplication>();
        settingsForInvoicingApplication.CsvFile = Path.Combine(executingDirectory, "TestData", "G1.csv");
        _sut = setup.GetService<IInvoicingHandler>();

        _outputDirectory = new DirectoryInfo(setup.GetSetting<SettingsForReporting>().OutputDirectory);
        CleanTestDataDirectory(_outputDirectory);
    }

    
    [Fact]
    public async Task? LoadInvoicesFromCSV_File()
    {
        using var cts = new CancellationTokenSource();

        var result = await _sut.LoadInvoices(cts.Token);
        _sut.Dispose();

        Assert.NotNull(result);
        Assert.Equal(0,result.Status);
        Assert.Equal(1,result.CountFails);

        Assert.Equal(1, CountFiles(_outputDirectory, "*_E_InvoiceReport.txt"));
        Assert.Equal(1, CountFiles(_outputDirectory, "*_I_InvoiceReport.txt"));
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}