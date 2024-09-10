using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.Application.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Utils;
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

        var doUpload = new Option<bool>("Upload", description: "Upload invoices from .csv file to e-conomic");
        doUpload.AddAlias("-u");
        rootCommand.AddOption(doUpload);

        var doDumpInvoices = new Option<bool>("Dump_invoices", description: "Dump booked invoices");
        doDumpInvoices.AddAlias("-d");
        rootCommand.AddOption(doDumpInvoices);

        var fromDate = new Option<string>("From-date", description: "Date interval to ");
        fromDate.AddAlias("-f");
        fromDate.ArgumentHelpName = "yyyy-mm-dd";
        rootCommand.AddOption(fromDate);

        var toDate = new Option<string>("To-date", description: "Date interval to ");
        toDate.AddAlias("-t");
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
        [InstantHandle] SetupBase setup,
        bool doUpload, bool doDumpInvoice, string fromDate, string toDate


        // ReSharper disable once UnusedParameter.Local
        , CancellationToken cancellationToken
    )
    {
        try
        {
            //var op1 = option1.

            if (doUpload && doDumpInvoice)
            {
                return new ExecutionStatus() { CountFails = 0, Report = $"Error! Both '{nameof(doUpload)}' and '{nameof(doDumpInvoice)}' are set", Status = -91 };
            }

            if (doUpload)
            {
                //var invoicingHandler = setup.GetService<IInvoicingHandler>();
                //var status = await invoicingHandler.LoadInvoices(cancellationToken);
                //return status;

                return new ExecutionStatus() { CountFails = 0, Report = $"Upload Invoices", Status = 0 };
            }
            else if (doDumpInvoice)
            {
                //Contract.DTO.Invoices.booked.get.
                var invoices = new List<Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.booked.get.BookedInvoice>();
                var gw = setup.GetService<IEconomicGatewayV2>();

                var from = DateTime.Parse(fromDate);
                var to = DateTime.Parse(toDate);

                var dateRange = Interval<DateTime>.Create(from, to);

                bool @continue = true;
                var page = 0;
                while (@continue)
                {
                    var bookedInvoicesHandle = await gw.ReadBookedInvoices(page, 20, dateRange, cancellationToken);
                    foreach (var invoice in bookedInvoicesHandle.Invoices)
                    {
                        invoices.Add(invoice);
                    }

                    @continue = bookedInvoicesHandle.Invoices.Any() && page < 100;
                    page++;
                }
                
                var sb = new StringBuilder();

                sb.AppendLine("CustomerNumber;InvoiceNumber;Date;Name-address;Product;Antal;Pris");
                foreach (var invoice in invoices.OrderBy(inv => inv.customer.customerNumber).ThenBy(inv => inv.bookedInvoiceNumber))
                {
                    var bi = await gw.ReadBookedInvoice(invoice.bookedInvoiceNumber, cancellationToken);
                    foreach (var line in bi.lines)
                    {
                        // TODO map to export format where it is possible to order by product number. then export to file in csv format.

                        sb.Append($"{bi.customer.customerNumber};{bi.bookedInvoiceNumber};{bi.date:yyyy-MM-dd};{bi.recipient.name}-{bi.recipient.address};{line.product.productNumber};{line.quantity};{line.totalNetAmount}");
                        sb.AppendLine();
                    }
                }

                var all = sb.ToString();
            

                return new ExecutionStatus() { CountFails = 0, Report = $"Dump invoices from {fromDate} to {toDate}", Status = 0 };
            }
            else
            {
                return new ExecutionStatus() { CountFails = 0, Report = $"No operation selected", Status = -98 };
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