﻿namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;

public interface IDraftInvoice
{
    int DraftInvoiceNumber { get; }

    double GrossAmount { get; }
}