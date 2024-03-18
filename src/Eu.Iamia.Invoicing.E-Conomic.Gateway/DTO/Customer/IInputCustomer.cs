namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

public interface IInputCustomer
{
    int    CustomerNumber { get; set; }

    int    PaymentTerms   { get; set; }

    string Address        { get; set; }

    string City           { get; set; }

    string Country        { get; set; }

    string Email          { get; set; }

    string Name           { get; set; }

    string Zip            { get; set; }

    int Group          { get; set; }
}