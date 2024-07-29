using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;

using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2;
public class GatewayV2 : IEconomicGatewayV2
{
    private readonly SettingsForEConomicGatewayV2 _settings;
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

    public CustomerCache CustomerCache { get; set; }

    public ProductCache ProductCache { get; set; }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??= new Mapper(new SettingsForEConomicGateway { LayoutNumber = _settings.LayoutNumber }, CustomerCache!, ProductCache!);


    public Task<CustomersHandle> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ProductsHandle> ReadProductsPaged(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        _report.SetCustomer(new CachedCustomer { Name = "---- ----", CustomerNumber = inputInvoice.CustomerNumber });

        const string reference = nameof(PushInvoice);

        var converted = Mapper.From(inputInvoice);

        var json = converted.ecInvoice.ToJson();
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

    public Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        throw new NotImplementedException();
    }

    public Task LoadProductCache()
    {
        throw new NotImplementedException();
    }
}
