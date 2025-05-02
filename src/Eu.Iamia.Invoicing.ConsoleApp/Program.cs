using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
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
    private const string OptExportInv = "--Export_invoices";
    private const string AliasExportInv = "-e";
    private const string OptFromDate = "--From-date";
    private const string AliasFromDate = "-f";
    private const string OptToDate = "--To-date";
    private const string AliasToDate = "-t";
    private const string OptIncludeNonInvCustomers = "--IncludeNonInvoicedCustomers";
    private const string AliasIncludeNonInvCustomers = "-inc";
    private const string OptExampleExport = "--Exmample_export";
    private const string AliasExampleExport = "-xe";


    private static async Task<int> Main(string[] args)
    {
        using var setup = new Setup(args);

        var rootCommand = new RootCommand("e-conomic debitor invoice service. - Console Application");

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

        var doDumpInvoices = new Option<bool>(OptExportInv, description: "Export booked invoices");
        doDumpInvoices.AddAlias(AliasExportInv);
        rootCommand.AddOption(doDumpInvoices);

        var fromDate = new Option<string>(OptFromDate, description: $"Date interval from incl. (required by {OptExportInv})");
        fromDate.AddAlias(AliasFromDate);
        fromDate.ArgumentHelpName = "yyyy-mm-dd";
        rootCommand.AddOption(fromDate);

        var toDate = new Option<string>(OptToDate, description: $"Date interval to incl. (required by {OptExportInv})");
        toDate.AddAlias(AliasToDate);
        toDate.ArgumentHelpName = "yyyy-mm-dd";
        rootCommand.AddOption(toDate);

        var doExampleExport = new Option<bool>(OptExampleExport, description: $"How to export debitor invoices");
        doExampleExport.AddAlias(AliasExampleExport);
        rootCommand.AddOption(doExampleExport);

        var doIncludeNonInvoicedCustomers = new Option<bool>(OptIncludeNonInvCustomers, description: $"Include Customers without invoice. (optional on {OptExportInv})");
        doIncludeNonInvoicedCustomers.AddAlias(AliasIncludeNonInvCustomers);
        doIncludeNonInvoicedCustomers.Arity = ArgumentArity.Zero;
        rootCommand.AddOption(doIncludeNonInvoicedCustomers);

        var psr = rootCommand.Parse(args);

        ExecutionStatus? result = null;

        var cts = new CancellationTokenSource();

        rootCommand.SetHandler(async (
                uploadFlagValue
                , dumpInvoiceFlagValue
                , fromDateValue
                , toDateValue
                , includeNonInvoicedCustomersValue
                , doExampleExportValue
            ) =>
            {
                //var cancellationToken = context.GetCancellationToken();
                // ReSharper disable once AccessToDisposedClosure
                result = await DefaultHandler(
                    setup,
                    uploadFlagValue,
                    dumpInvoiceFlagValue,
                    fromDateValue,
                    toDateValue,
                    includeNonInvoicedCustomersValue,
                    doExampleExportValue,
                    cts.Token
                );
            },
            doUpload, doDumpInvoices, fromDate, toDate, doIncludeNonInvoicedCustomers, doExampleExport
        );

        var commandLineBuilder = new CommandLineBuilder(rootCommand);

        var exampleDirective = new[] { "Example", "Examples", "Ex" };

        commandLineBuilder.AddMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Directives.Contains("just-say-hi"))
                {
                    context.Console.WriteLine("Hi!");
                }
                else if (exampleDirective.Any(probe => context.ParseResult.Directives.Any(dir => dir.Key.ToLowerInvariant().Equals(probe.ToLowerInvariant()))))
                {
                    context.Console.WriteLine($"Example");
                }
                else
                {
                    await next(context);
                }
            }
        );

        commandLineBuilder.UseDefaults();
        var parser = commandLineBuilder.Build();
        await parser.InvokeAsync(args);

        //await rootCommand.InvokeAsync(args); // this must be before the test for errors.

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
        , bool? includeNonInvoicedCustomers
        , bool doExampleExport
        , CancellationToken cancellationToken
    )
    {
        try
        {
            if (doUpload && doDumpInvoice)
            {
                return new ExecutionStatus { Report = $"Error! Both '{OptUpload}/{AliasUpload}' and '{OptExportInv}/{AliasExportInv}' are specified", Status = -91 };
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
                    return new ExecutionStatus { Report = $"Error! '{OptFromDate}/{AliasFromDate}' must be specified together with '{OptExportInv}/{AliasExportInv}''", Status = -92 };
                }

                if (!DateTime.TryParse(toDate, out var to))
                {
                    return new ExecutionStatus { CountFails = 1, Report = $"Error! '{OptToDate}/{AliasToDate}' must be specified together with '{OptExportInv}/{AliasExportInv}'", Status = -93 };
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

                var outputFile = new FileInfo($"C:\\Development\\Logfiles\\{DateTime.Now:yyyy-MM-dd_HH-mm}_BookedInvoices.csv");
                var executionStatus = await exportService.ExportBookedInvoices(dateRange, includeNonInvoicedCustomers ?? false, outputFile, cancellationToken);

                return executionStatus;
            }

            if (doExampleExport)
            {
                var year = DateTime.Today.Year;
                return new ExecutionStatus { Report = $"Export booked debitor invoices: Eu.Iamia.Invoicing.ConsoleApp -d -f {year}-01-01 -t {year}-12-31", Status = 0 };
            }

            {
                return new ExecutionStatus { Report = $"No option selected, provide option -h for help", Status = -98 };
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