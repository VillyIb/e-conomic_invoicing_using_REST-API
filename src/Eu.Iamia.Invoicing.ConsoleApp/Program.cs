using System.CommandLine;
using System.CommandLine.Parsing;
using System.Globalization;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.Application;
using JetBrains.Annotations;

namespace Eu.Iamia.Invoicing.ConsoleApp;

public class Program
{
    private static async Task<int> Main(string[] args)
    {
        using var setup = new Setup(args);

        var rootCommand = new RootCommand("Upload af fakturaer til e-conomic - Console Application");

        foreach (var helpMetadata in setup.HelpMetaData)
        {
            var option = new Option<string>(name: helpMetadata.Name, description: helpMetadata.Description)
            {
                ArgumentHelpName = helpMetadata.ArgumentHelpName
            };

            foreach (var alias in helpMetadata.AliasList)
            {
                option.AddAlias(alias);
            }

            rootCommand.AddOption(option);
        }

        var bookingDataOption = new Option<string>(description: "Bogførings dato", name: "--bogføringsdato")
        {
            ArgumentHelpName = "yyyy-MM-dd"
        };

        bookingDataOption.AddAlias("-d");
        rootCommand.AddOption(bookingDataOption);

        var psr = rootCommand.Parse(args);

        ExecutionStatus result = null;

        //rootCommand.SetHandler((bookingDateValue) =>
        //    {
        //        // ReSharper disable once AccessToDisposedClosure
        //        result = DefaultHandler(setup, setupBase => setupBase.GetService<IInvoicingHandler>(), bookingDateValue);
        //    }, bookingDataOption
        //);
        
        rootCommand.SetHandler(async (context) =>
            {
                var bookingDateValue = context.ParseResult.GetValueForOption(bookingDataOption);
                var cancellationToken = context.GetCancellationToken();
                // ReSharper disable once AccessToDisposedClosure
                result = await DefaultHandler(setup, setupBase => setupBase.GetService<IInvoicingHandler>(), bookingDateValue, cancellationToken);
            }
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

            if (result.status == 0)
            {
                Console.WriteLine($"Successful");
            }
            else
            {
                 Console.WriteLine($"Failed with status: {result.status}");
            }

            Console.WriteLine();

            return result.status;
        }

        if (IsOptionSpecified(psr.Tokens, bookingDataOption))
        {
            return 0;
        }

        //var app = setup.GetService<IApplication>();
        //try
        //{
        //    app.Execute();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine();
        //    Console.WriteLine(ex.Message);
        //    Console.WriteLine(ex.StackTrace);
        //    return 1;
        //}
        //Console.WriteLine("\n\nDone\n");

        return -19;
    }

    internal static bool IsOptionSpecified(IReadOnlyList<Token> parserTokens, Option option)
    {
        return option.Aliases.Any(alias => parserTokens.Any(token => token.Equals(new Token(alias, TokenType.Option, option))));
    }

    //private static  ExecutionStatus DefaultHandler([InstantHandle] SetupBase setup, Func<SetupBase, IInvoicingHandler> getService, string bookingDate)
    //{
    //    try
    //    {
    //        if (string.IsNullOrWhiteSpace(bookingDate))
    //        {
    //            return new ExecutionStatus
    //            {
    //                Report = "Missing booking date",
    //                status = -1
    //            };
    //        }

    //        if (DateTime.TryParseExact(bookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
    //                DateTimeStyles.AssumeLocal, out DateTime date))

    //        {
    //            var reportDate = date;
    //            var invoicingHandler = getService.Invoke(setup);
    //            var status = await invoicingHandler.LoadInvoices(reportDate);
    //            return status;
    //        }

    //        return new ExecutionStatus
    //        {
    //            Report = $"Unable to parse booking date '{bookingDate}' please specify as YYYY-MM-DD",
    //            status = -1
    //        };
    //    }
    //    catch (OperationCanceledException)
    //    {
    //        Console.Error.WriteLineAsync("The operation was aborted");
            
    //        return new ExecutionStatus
    //        {
    //            Report = "The operation was aborted",
    //            status = -1
    //        }; 
    //    }
    //}

    private static async Task<ExecutionStatus> DefaultHandler(
        [InstantHandle] SetupBase setup, 
        Func<SetupBase, IInvoicingHandler> getService, 
        string? bookingDate, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(bookingDate))
            {
                return new ExecutionStatus
                {
                    Report = "Missing booking date",
                    status = -1
                };
            }

            if (DateTime.TryParseExact(bookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal, out DateTime date))

            {
                var reportDate = date;
                var invoicingHandler = getService.Invoke(setup);
                var status = await invoicingHandler.LoadInvoices(reportDate);
                return status;
            }

            return new ExecutionStatus
            {
                Report = $"Unable to parse booking date '{bookingDate}' please specify as YYYY-MM-DD",
                status = -2
            };
        }
        catch (OperationCanceledException)
        {
            await Console.Error.WriteLineAsync("The operation was aborted");

            return new ExecutionStatus
            {
                Report = "The operation was aborted",
                status = -3
            };
        }
    }
}