using System.Runtime.InteropServices.JavaScript;

namespace Eu.Iamia.Invoicing.Application;

public interface IInvoicingHandler
{
    ExecutionStatus LoadInvoices(DateTime invoiceDate );
}

public class InvoicingHandler : IInvoicingHandler
{
    public ExecutionStatus LoadInvoices(DateTime invoiceDate)
    {
        return new ExecutionStatus{ Report = $"Report status {invoiceDate:G}", status = -1};
    }
}

public class ExecutionStatus
{
    public int status { get; set; } = 0;

    public string Report { get; set; } = string.Empty;
}
