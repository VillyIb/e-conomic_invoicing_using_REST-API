namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;

public interface ICachedCustomer
{
    int     CustomerNumber { get; init; }

    int     PaymentTerms   { get; init; }

    string? Address        { get; init; }

    string? City           { get; init; }

    string? Name           { get; init; }

    string? Zip            { get; init; }
}