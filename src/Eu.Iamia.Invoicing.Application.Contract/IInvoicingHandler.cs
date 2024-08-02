namespace Eu.Iamia.Invoicing.Application.Contract;

public interface IInvoicingHandler : IDisposable
{
    Task<ExecutionStatus> LoadInvoices(CancellationToken cancellationToken);
}