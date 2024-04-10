namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

using Eu.Iamia.Invoicing.Loader.Contract;

public interface IEconomicGateway
{
    Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber);

    Task LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task LoadProcuctCache();

    Task<string> ReadCustomersPaged(int page, int pageSize);

    Task<string> ReadProductsPaged(int page, int pageSize);

    Task<string> ReadInvoice();
}

public interface IDraftInvoice
{
    int DraftInvoiceNumber { get; set; }

    double GrossAmount { get; set; }
}