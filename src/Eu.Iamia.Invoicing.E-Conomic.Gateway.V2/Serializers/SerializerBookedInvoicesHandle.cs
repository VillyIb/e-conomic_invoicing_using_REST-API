﻿using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.BookedInvoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;

public class SerializerBookedInvoicesHandle : ISerializerBookedInvoiceHandle
{
    public async Task<BookedInvoiceHandle> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken)
    {
        return await JsonSerializer.DeserializeAsync<BookedInvoiceHandle>(utf8Json, new JsonSerializerOptions(), cancellationToken) ?? new BookedInvoiceHandle();
    }
}
