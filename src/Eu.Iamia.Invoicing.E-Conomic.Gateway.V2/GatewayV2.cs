﻿using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customers.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Customers.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Invoices.drafts.draftInvoiceNumber.lines.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers.Products.get;
using Eu.Iamia.Utils.Contract;


namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;

public class GatewayV2 : IEconomicGatewayV2
{
    // ReSharper disable once NotAccessedField.Local
    private readonly SettingsForEConomicGatewayV2 _settings; // TODO review unused local
    protected readonly IRestApiGateway RestApiGateway;
    private readonly ICustomerReport _report;

    public GatewayV2(
        SettingsForEConomicGatewayV2 settings,
        IRestApiGateway restApiGateway,
        ICustomerReport report
    )
    {
        if (restApiGateway == null) throw new ArgumentNullException(nameof(restApiGateway));
        if (report == null) throw new ArgumentNullException(nameof(report));
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        _settings = settings;
        RestApiGateway = restApiGateway;
        _report = report;
    }

    public GatewayV2(
        IOptions<SettingsForEConomicGatewayV2> settings,
        IRestApiGateway restApiGateway,
        ICustomerReport report
    ) : this(settings.Value, restApiGateway, report)
    { }


    public async Task<CustomersHandle> ReadCustomers(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetCustomers(page, pageSize, cancellationToken);

        var serializerCustomersHandle = new SerializerCustomersHandle();

        var customersHandle = await serializerCustomersHandle.DeserializeAsync(stream, cancellationToken);

        return customersHandle;
    }

    public async Task<ProductsHandle> ReadProducts(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetProducts(page, pageSize, cancellationToken);

        var serializerProductsHandle = new SerializerProductsHandle();

        var productsHandle = await serializerProductsHandle.DeserializeAsync(stream, cancellationToken);

        return productsHandle;
    }

    public async Task<IDraftInvoice?> PostDraftInvoice(Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PostDraftInvoice);

        var json = restApiInvoice.ToJson();

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var stream = await RestApiGateway.PostDraftInvoice(content, cancellationToken);

        var serializerDraftInvoice = new SerializerDraftInvoice();

        var draftInvoice = await serializerDraftInvoice.DeserializeAsync(stream, cancellationToken);

        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var htmlBody = await streamReader.ReadToEndAsync(cancellationToken);
        _report.Info(reference, htmlBody);
        _report.Close();

        return draftInvoice;
    }

    public async Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int page, int pageSize, IInterval<DateTime> dateRange, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetBookedInvoices(page, pageSize, dateRange, cancellationToken);

        var serializerCustomersHandle = new Serializers.Invoices.booked.get.SerializerBookedInvoicesHandle();

        var customersHandle = await serializerCustomersHandle.DeserializeAsync(stream, cancellationToken);

        return customersHandle ?? new();
    }

    public async Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice> ReadBookedInvoice(int invoiceNumber, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetBookedInvoice(invoiceNumber, cancellationToken);

        var serializerCustomersHandle = new Serializers.Invoices.booked.bookedInvoiceNumber.get.SerializerBookedInvoice();

        var bookedInvoice = await serializerCustomersHandle.DeserializeAsync(stream, cancellationToken);

        return bookedInvoice;
    }

    private readonly List<PaymentTerm> _paymentTermsCache = [];

    public async Task<PaymentTermsHandle> ReadPaymentTerms(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetPaymentTerms(page, pageSize, cancellationToken);

        var serializerPaymentTermsHandle = new SerializerPaymentTermsHandle();

        var paymentTermsHandle = await serializerPaymentTermsHandle.DeserializeAsync(stream, cancellationToken);

        return paymentTermsHandle ?? new();
    }
}
