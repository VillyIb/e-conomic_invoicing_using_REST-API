namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

public static class InvoiceGuards
{
    public static void CustomerNumber(int customerNumber)
    {
        const int minValue = 1;
        const int maxValue = 999999999;

        if (minValue <= customerNumber && customerNumber <= maxValue) { return; }
        throw new ArgumentOutOfRangeException(nameof(customerNumber), customerNumber, $"Valid range {{{minValue}..{maxValue}}}");
    }

    public static void DeliveryAddress(string? address)
    {
        const int maxNumberOfCharacters = 255;

        if (address.Length <= maxNumberOfCharacters) { return; }
        throw new ArgumentOutOfRangeException(nameof(address), address, $"Valid number of characters 0..{maxNumberOfCharacters}");
    }

    public static void DeliveryZip(string? zip)
    {
        const int maxNumberOfCharacters = 30;

        if (zip.Length <= maxNumberOfCharacters) { return; }
        throw new ArgumentOutOfRangeException(nameof(zip), zip, $"Valid number of characters 0..{maxNumberOfCharacters}");
    }

    public static void DeliveryCity(string? city)
    {
        const int maxNumberOfCharacters = 50;

        if (city.Length <= maxNumberOfCharacters) { return; }
        throw new ArgumentOutOfRangeException(nameof(city), city, $"Valid number of characters 0..{maxNumberOfCharacters}");
    }
    public static void DeliveryCountry(string? country)
    {
        const int maxNumberOfCharacters = 50;

        if (country.Length <= maxNumberOfCharacters) { return; }
        throw new ArgumentOutOfRangeException(nameof(country), country, $"Valid number of characters 0..{maxNumberOfCharacters}");
    }
}
