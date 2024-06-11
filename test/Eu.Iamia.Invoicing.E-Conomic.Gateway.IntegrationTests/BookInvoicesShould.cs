using System.Reflection;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class BookInvoicesShould
{
    private static readonly SettingsForEConomicGateway SettingsDemo = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        LayoutNumber = 21,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };

    private SettingsForEConomicGateway SettingsReal { get; init; }

    private static ICustomerReport CustomerReport => new MockedReport();

    // ReSharper disable once MemberCanBeMadeStatic.Local
    // ReSharper disable once InconsistentNaming
    private (IList<IInputInvoice> Invoices, IList<int> CustomerGroupsToAccept) LoadCSV()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests.TestData.G1.csv";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        var loader = new CSVLoader.Loader();

        var __ = loader.ParseCSV(stream!);

        return (loader.Invoices!, loader.CustomerGroupToAccept!);
    }

    public async Task<CustomerCache> GetCustomerCache(GatewayBase gatewayInvoice, IList<int> customerGroupsToAccept)
    {
        var customerCache = new CustomerCache(gatewayInvoice, customerGroupsToAccept);
        await customerCache.LoadAllCustomers();

        return customerCache;
    }
    public async Task<ProductCache> GetProductCache(GatewayBase gatewayInvoice)
    {

        var productCache = new ProductCache(gatewayInvoice);
        await productCache.LoadAllProducts();

        return productCache;
    }

    public BookInvoicesShould()
    {
        using var setup = new Setup(null);
        SettingsReal = setup.GetSetting<SettingsForEConomicGateway>();
    }

    [Fact]
    public async Task GivenDemoAuthentication_BookInvoices_In_CsvFile()
    {
        var invoiceDate = DateTime.Today;

        var csv = LoadCSV();
        var invoices = csv.Invoices;
        var customerGroupsToAccept = csv.CustomerGroupsToAccept;
        Assert.NotNull(invoices);
        Assert.True(invoices.Any());

        var serializer = new JsonSerializerFacade();

        var gatewayInvoice = new GatewayBaseStub(
            SettingsDemo,
            new SerializerCustomersHandle(serializer),
            new SerializerDeletedInvoices(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            CustomerReport,
            new HttpClientHandler()
        );
        var customerCache = await GetCustomerCache(gatewayInvoice, customerGroupsToAccept);
        var productCache = await GetProductCache(gatewayInvoice);

        var mapper = new Mapper(SettingsDemo, customerCache, productCache);

        foreach (var inputInvoice in invoices)
        {
            inputInvoice.InvoiceDate = invoiceDate;

            var invoice = mapper.From(inputInvoice);

            // TODO fails, dont work on Demo Authentication...
            var response = await gatewayInvoice.PushInvoice(invoice.customer, invoice.ecInvoice, inputInvoice.SourceFileLineNumber);

            Assert.NotNull(response);

            Assert.Equal(-1, response.DraftInvoiceNumber);
        }
    }

    [Fact]
    public async Task GivenRealAuthentication_ReadDraftInvoice_Successfully()
    {
        var serializer = new JsonSerializerFacade();

        var gatewayInvoice = new GatewayBaseStub(
            SettingsReal,
            new SerializerCustomersHandle(serializer),
            new SerializerDeletedInvoices(serializer),
            new SerializerDraftInvoice(serializer),
            new SerializerProductsHandle(serializer),
            CustomerReport,
            new HttpClientHandler()
            );
        var customerCache = await GetCustomerCache(gatewayInvoice, new List<int> { 1, 3 });
        var productCache = await GetProductCache(gatewayInvoice);

        var x = await gatewayInvoice.GetDraftInvoice(373);


    }
}
