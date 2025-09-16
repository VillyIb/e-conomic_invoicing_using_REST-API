using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils.Contract;
using Microsoft.Extensions.Options;
using System.Text;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;

public partial class GatewayV2 : IEconomicGatewayV2
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

    public async Task<Contract.DTO.Customers.get.CustomersHandle?> ReadCustomers(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetCustomers(page, pageSize, cancellationToken);

        var customersHandle = await GenericSerializer<Contract.DTO.Customers.get.CustomersHandle>.DeserializeAsync(stream, cancellationToken);

        return customersHandle;
    }

    public async Task<Contract.DTO.Products.get.ProductsHandle> ReadProducts(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetProducts(page, pageSize, cancellationToken);

        var productsHandle = await GenericSerializer<Contract.DTO.Products.get.ProductsHandle>.DeserializeAsync(stream, cancellationToken);

        return productsHandle ?? new Contract.DTO.Products.get.ProductsHandle() { Products = new List<Contract.DTO.Products.get.Product>(0)};
    }

    public async Task<Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice?> PostDraftInvoice(Contract.DTO.Invoices.drafts.post.Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken)
    {
        var json = Contract.DTO.Invoices.drafts.post.InvoiceExtension.ToJson(restApiInvoice);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var stream = await RestApiGateway.PostDraftInvoice(content, cancellationToken);

        var draftInvoice = await GenericSerializer<Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post.DraftInvoice>.DeserializeAsync(stream, cancellationToken);

        return draftInvoice;
    }

    public async Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int page, int pageSize, IInterval<DateTime> dateRange, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetBookedInvoices(page, pageSize, dateRange, cancellationToken);

        var customersHandle = await GenericSerializer<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle>.DeserializeAsync(stream, cancellationToken);

        return customersHandle ?? new();
    }

    public async Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice?> ReadBookedInvoice(int invoiceNumber, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetBookedInvoice(invoiceNumber, cancellationToken);

        var bookedInvoice = await GenericSerializer<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice>.DeserializeAsync(stream, cancellationToken);

        return bookedInvoice;
    }

    private readonly List<Contract.DTO.PaymentTerms.get.PaymentTerm> _paymentTermsCache = [];

    public async Task<Contract.DTO.PaymentTerms.get.PaymentTermsHandle> ReadPaymentTerms(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetPaymentTerms(page, pageSize, cancellationToken);

        var paymentTermsHandle = await GenericSerializer<Contract.DTO.PaymentTerms.get.PaymentTermsHandle>.DeserializeAsync(stream, cancellationToken);

        return paymentTermsHandle ?? new() { PaymentTerms = Array.Empty<Contract.DTO.PaymentTerms.get.PaymentTerm>()};
    }

    public async Task<Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice?> PostDraftInvoice(Contract.DTO.Invoices.drafts.post.Invoice draftInvoice)
    {
       

        throw new NotImplementedException();
    }

    public async Task<Contract.DTO.Invoices.drafts.get.DraftInvoicesHandle> GetDraftInvoices(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await RestApiGateway.GetDraftInvoices(page, pageSize, cancellationToken);

        var customersHandle = await GenericSerializer<Contract.DTO.Invoices.drafts.get.DraftInvoicesHandle>.DeserializeAsync(stream, cancellationToken);

        return customersHandle ?? new() { Invoices = Array.Empty<Contract.DTO.Invoices.drafts.get.DraftInvoice>() };
    }

}
