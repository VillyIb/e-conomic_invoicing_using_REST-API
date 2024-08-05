namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

public interface IEconomicGateway
{
   
}

public interface IDraftInvoice
{
    int DraftInvoiceNumber { get; }

    double GrossAmount { get; }
}