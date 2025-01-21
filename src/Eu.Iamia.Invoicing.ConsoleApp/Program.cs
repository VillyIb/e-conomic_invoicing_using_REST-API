using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.Application;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Utils;
using Eu.Iamia.Utils.Contract;
using JetBrains.Annotations;
using ArgumentException = System.ArgumentException;

namespace Eu.Iamia.Invoicing.ConsoleApp;

public class Program
{
    private const string OptUpload = "--Upload";
    private const string AliasUpload = "-u";
    private const string OptDumpInv = "--Dump_invoices";
    private const string AliasDumpInv = "-d";
    private const string OptFromDate = "--From-date";
    private const string AliasFromDate = "-f";
    private const string OptToDate = "--To-date";
    private const string AliasToDate = "-t";

    private static async Task<int> Main(string[] args)
    {
        using var setup = new Setup(args);

        var rootCommand = new RootCommand("Upload af fakturaer til e-conomic m.m. - Console Application");

        foreach (var helpMetadata in setup.HelpMetaData)
        {
            var option = new Option<string>(name: helpMetadata.Name, description: helpMetadata.Description)
            {
                ArgumentHelpName = helpMetadata.ArgumentHelpName,
                IsRequired = helpMetadata.IsRequired,
            };
            rootCommand.AddOption(option);
        }

        var doUpload = new Option<bool>(OptUpload, description: "Upload invoices from .csv file to e-conomic");
        doUpload.AddAlias(AliasUpload);
        rootCommand.AddOption(doUpload);

        var doDumpInvoices = new Option<bool>(OptDumpInv, description: "Dump booked invoices");
        doDumpInvoices.AddAlias(AliasDumpInv);
        rootCommand.AddOption(doDumpInvoices);

        var fromDate = new Option<string>(OptFromDate, description: "Date interval from incl.");
        fromDate.AddAlias(AliasFromDate);
        fromDate.ArgumentHelpName = "yyyy-mm-dd";
        rootCommand.AddOption(fromDate);

        var toDate = new Option<string>(OptToDate, description: "Date interval to incl.");
        toDate.AddAlias(AliasToDate);
        toDate.ArgumentHelpName = "yyyy-mm-dd";
        rootCommand.AddOption(toDate);

        var psr = rootCommand.Parse(args);

        ExecutionStatus? result = null;

        var cts = new CancellationTokenSource();

        rootCommand.SetHandler(async (uploadFlagValue, dumpInvoiceFlagValue, fromDateValue, toDateValue) =>
            {
                //var cancellationToken = context.GetCancellationToken();
                // ReSharper disable once AccessToDisposedClosure
                result = await DefaultHandler(
                    setup,
                    uploadFlagValue,
                    dumpInvoiceFlagValue,
                    fromDateValue,
                    toDateValue,
                    cts.Token
                );
            },
            doUpload, doDumpInvoices, fromDate, toDate
        );

        await rootCommand.InvokeAsync(args); // this must be before the test for errors.

        if (psr.Errors.Any())
        {
            return 1;
        }

        if (result != null)
        {
            Console.WriteLine();
            Console.WriteLine($"{result.Report}");

            if (result.Status == 0)
            {
                Console.WriteLine($"Successful");
            }
            else
            {
                Console.WriteLine($"Failed with status: {result.Status}");
            }

            Console.WriteLine();

            return result.Status;
        }

        return -19;
    }

    internal static bool IsOptionSpecified(IReadOnlyList<Token> parserTokens, Option option)
    {
        return option.Aliases.Any(alias => parserTokens.Any(token => token.Equals(new Token(alias, TokenType.Option, option))));
    }

    private static async Task<ExecutionStatus> DefaultHandler(
        [InstantHandle] SetupBase setup
        , bool doUpload
        , bool doDumpInvoice
        , string fromDate
        , string toDate
        , CancellationToken cancellationToken
    )
    {
        try
        {
            if (doUpload && doDumpInvoice)
            {
                return new ExecutionStatus { Report = $"Error! Both '{OptUpload}/{AliasUpload}' and '{OptDumpInv}/{AliasDumpInv}' are specified", Status = -91 };
            }

            if (doUpload)
            {
                var invoicingHandler = setup.GetService<IInvoicingHandler>();
                var executionStatus = await invoicingHandler.LoadInvoices(cancellationToken);
                return executionStatus;
            }

            if (doDumpInvoice)
            {
                if (!DateTime.TryParse(fromDate, out var from))
                {
                    return new ExecutionStatus { Report = $"Error! '{OptFromDate}/{AliasFromDate}' must be specified together with '{OptDumpInv}/{AliasDumpInv}''", Status = -92 };
                }

                if (!DateTime.TryParse(toDate, out var to))
                {
                    return new ExecutionStatus { CountFails = 1, Report = $"Error! '{OptToDate}/{AliasToDate}' must be specified together with '{OptDumpInv}/{AliasDumpInv}'", Status = -93 };
                }

                var exportService = setup.GetService<IExportService>();

                IInterval<DateTime> dateRange;
                try
                {
                    dateRange = Interval<DateTime>.Create(from, to);
                }
                catch (ArgumentException)
                {
                    return new ExecutionStatus { Report = $"Error! '{OptFromDate}/{AliasFromDate}' ({fromDate}) > '{OptToDate}/{AliasToDate}' ({toDate})", Status = -94 };
                }

                var executionStatus = await exportService.ExportBookedInvoices(dateRange, cancellationToken);

                return executionStatus;
            }

            {
                return new ExecutionStatus { Report = $"No option selected", Status = -98 };
            }
        }
        catch (ApplicationException ex)
        {
            return new ExecutionStatus
            {
                Report = ex.Message,
                Status = -9
            };
        }
        catch (OperationCanceledException)
        {
            return new ExecutionStatus
            {
                Report = "The operation was aborted",
                Status = -3
            };
        }
    }
}