namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
public static class CustomerDtoExtension
{
    public static bool IsClosed(this Collection customer) => customer.customerGroup.customerGroupNumber == 99; // Warning setup specific logic.

    public static bool IsDismissed(this Collection customer) => customer.customerGroup.customerGroupNumber == 3; // Warning setup specific logic.
}
