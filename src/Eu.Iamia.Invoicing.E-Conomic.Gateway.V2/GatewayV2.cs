using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Serializers;
using Eu.Iamia.Utils;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.Serializers;


namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;
public class GatewayV2 : IEconomicGatewayV2
{
    private readonly SettingsForEConomicGatewayV2 _settings; // TODO review
    private readonly IRestApiGateway _restApiGateway;
    private readonly ICustomerReport _report;

    public GatewayV2(
        SettingsForEConomicGatewayV2 settings,
        IRestApiGateway restApiGateway,
        ICustomerReport report
    )
    {
        _settings = settings;
        _restApiGateway = restApiGateway;
        _report = report;
    }

    public GatewayV2(
        IOptions<SettingsForEConomicGatewayV2> settings,
        IRestApiGateway restApiGateway,
        ICustomerReport report
    ) : this(settings.Value, restApiGateway, report)
    { }


    public async Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await _restApiGateway.GetCustomersPaged(page, pageSize, cancellationToken);

        var serializer = new JsonSerializerFacade();
        var serializerCustomersHandle = new SerializerCustomersHandle(serializer);

        var customersHandle = await serializerCustomersHandle.DeserializeAsync(stream, cancellationToken);

        return customersHandle;
    }

    public async Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var stream = await _restApiGateway.GetProductsPaged(page, pageSize, cancellationToken);

        var serializer = new JsonSerializerFacade();
        var serializerProductsHandle = new SerializerProductsHandle(serializer);

        var productsHandle = await serializerProductsHandle.DeserializeAsync(stream, cancellationToken);

        return productsHandle;
    }

    public async Task<IDraftInvoice?> PushInvoice(Contract.DTO.Invoice.Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PushInvoice);

        var json = Contract.DTO.Invoice.InvoiceExtension.ToJson(restApiInvoice);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var stream = await _restApiGateway.PushInvoice(content, cancellationToken);


        var serializer = new JsonSerializerFacade();
        var serializerDraftInvoice = new SerializerDraftInvoice(serializer);

        var draftInvoice = await serializerDraftInvoice.DeserializeAsync(stream, cancellationToken);

        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        string htmlBody = await streamReader.ReadToEndAsync(cancellationToken);
        _report.Info(reference, htmlBody);
        _report.Close();

        return draftInvoice;
    }
}
