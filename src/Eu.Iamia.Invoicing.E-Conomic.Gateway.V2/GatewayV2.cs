using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
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

    [Obsolete("", true)]
    public CustomerCache CustomerCache { get; set; }

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

    [Obsolete]
    public async Task<IDraftInvoice?> PushInvoice(E_Conomic.Gateway.DTO.Invoice.Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PushInvoice);

        var json = restApiInvoice.ToJson();
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

    [Obsolete]
    public async Task<IDraftInvoice?> PushInvoice ( Application.Contract.DTO.InvoiceLineDto inputInvoice, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        //const string reference = nameof(PushInvoice);

        //var customerDto = CustomersCache.GetCustomer(inputInvoice.CustomerNumber);

        //_report.SetCustomer(new CachedCustomer
        //{
        //    Name = customerDto is null  ? "---- ----" : customerDto.Name, 
        //    CustomerNumber = inputInvoice.CustomerNumber
        //});

        //if (customerDto is null)
        //{
        //    throw new ApplicationException($"Customer does not exist: '{inputInvoice.CustomerNumber}', Source file line: {sourceFileLineNumber}");
        //}

        //var invoiceDto = new InvoiceDto
        //{
        //    CustomerNumber = inputInvoice.CustomerNumber,
        //    InvoiceDate = inputInvoice.InvoiceDate,
        //    SourceFileLineNumber = sourceFileLineNumber,
        //    PaymentTerm = inputInvoice.PaymentTerm,
        //    Text1 = inputInvoice.Text1
        //};
        //foreach (var inputLine in inputInvoice.InvoiceLines)
        //{
        //    invoiceDto.InvoiceLines.Add(
        //        new InvoiceLineDto
        //        {
        //            UnitNumber = inputLine.UnitNumber,
        //            ProductNumber = inputLine.ProductNumber,
        //            SourceFileLineNumber =sourceFileLineNumber,
        //            Description = inputLine.Description,
        //            Quantity = inputLine.Quantity,
        //            UnitNetPrice = inputLine.UnitNetPrice,
        //            UnitText = inputLine.UnitText
        //        }    
        //    );
        //}

        //var restApiInvoice = Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Mappings.Mapping.ToRestApiInvoice(
        //     customerDto, 
        //     invoiceDto, 
        //     ProductsCache, 
        //     _settings.LayoutNumber
        //);


        //var json = restApiInvoice.ToJson();
        //var content = new StringContent(json, Encoding.UTF8, "application/json");

        //var stream = await _restApiGateway.PushInvoice(content, cancellationToken);


        //var serializer = new JsonSerializerFacade();
        //var serializerDraftInvoice = new SerializerDraftInvoice(serializer);

        //var draftInvoice = await serializerDraftInvoice.DeserializeAsync(stream, cancellationToken);

        //stream.Position = 0;
        //using var streamReader = new StreamReader(stream);
        //string htmlBody = await streamReader.ReadToEndAsync(cancellationToken);
        //_report.Info(reference, htmlBody);
        //_report.Close();

        //return draftInvoice;
    }

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        throw new NotImplementedException();

        //CustomersCache.Clear();

        //var cts = new CancellationTokenSource();
        //bool @continue = true;
        //var page = 0;
        //while (@continue)
        //{
        //    var customersHandle = await ReadCustomersPaged(page, 20, cts.Token);
        //    foreach (var collection in customersHandle.collection)
        //    {
        //        if(!customerGroupsToAccept.Any(cg => cg.Equals(collection.customerGroup.customerGroupNumber))) continue;

        //        var customerDto = Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Mappings.Mapping.ToCustomerDto(collection);
        //        CustomersCache.Add(customerDto);
        //    }
        //    @continue = customersHandle.collection.Any() && page < 100;
        //    page++;
        //}
    }


    public async Task LoadProductCache()
    {
        throw new NotImplementedException();

        //ProductsCache.Clear();

        //var cts = new CancellationTokenSource();
        //bool @continue = true;
        //var page = 0;
        //while (@continue)
        //{
        //    var productsHandle = await ReadProductsPaged(page, 20, cts.Token);
        //    foreach (var collection in productsHandle.collection)
        //    {
        //        var productDto = Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Mappings.Mapping.ToProductDto(collection);
        //        ProductsCache.Add(productDto);
        //    }
        //    @continue = productsHandle.collection.Any() && page < 100;
        //    page++;
        //}
    }
}
