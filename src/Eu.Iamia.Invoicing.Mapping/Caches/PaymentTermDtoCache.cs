using Eu.Iamia.Invoicing.Application.Contract.DTO;

namespace Eu.Iamia.Invoicing.Mapping.Caches;

public class PaymentTermDtoCache : List<PaymentTermDto>
{
    public PaymentTermDto? GetPaymentTerm(string? paymentTermNumber)
    {
        return this.FirstOrDefault(pmt => pmt.PaymentTermNumber.Equals(paymentTermNumber, StringComparison.OrdinalIgnoreCase));
    }
}