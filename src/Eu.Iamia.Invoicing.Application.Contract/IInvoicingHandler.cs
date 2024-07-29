namespace Eu.Iamia.Invoicing.Application.Contract;

public interface IInvoicingHandler
{
    Task<ExecutionStatus> LoadInvoices();
}