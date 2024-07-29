using System.CommandLine;
using System.CommandLine.Parsing;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.Application;
using Eu.Iamia.Invoicing.Application.Contract;
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
                ArgumentHelpName = helpMetadata.ArgumentHelpName,
                IsRequired = helpMetadata.IsRequired,
            };

            rootCommand.AddOption(option);
        }

        var psr = rootCommand.Parse(args);

        ExecutionStatus? result = null;


        rootCommand.SetHandler(async (context) =>
            {
                var cancellationToken = context.GetCancellationToken();
                // ReSharper disable once AccessToDisposedClosure
                result = await DefaultHandler(setup, setupBase => setupBase.GetService<IInvoicingHandler>(), cancellationToken);
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
        [InstantHandle] SetupBase setup,
        Func<SetupBase, IInvoicingHandler> getService,
        // ReSharper disable once UnusedParameter.Local
        CancellationToken cancellationToken
    )
    {
        try
        {
            var invoicingHandler = getService.Invoke(setup);
            var status = await invoicingHandler.LoadInvoices();
            return status;
        }
        catch (OperationCanceledException)
        {
            await Console.Error.WriteLineAsync("The operation was aborted");

            return new ExecutionStatus
            {
                Report = "The operation was aborted",
                Status = -3
            };
        }
    }
}