namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

using Eu.Iamia.Invoicing.Loader.Contract;

public interface IEconomicGateway
{
   

    Task<string> PushInvoice(IInputInvoice invoice);

    Task LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task LoadProcuctCache();

}