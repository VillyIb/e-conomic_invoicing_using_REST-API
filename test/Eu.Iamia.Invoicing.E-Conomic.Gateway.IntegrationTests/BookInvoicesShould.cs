using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class BookInvoicesShould
{
    private static SettingsForEConomicGateway _settings = new SettingsForEConomicGateway
    {
        PaymentTerms = 1,
        X_AgreementGrantToken = "Demo",
        X_AppSecretToken = "Demo"
    };

    private static ICustomerReport CustomerReport => new MockedReport();

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private (IList<IInputInvoice> Invoices, IList<int> CustomerGroupsToAccept) LoadCSV()
    {
        var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\G1.csv");
        //var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\G2.csv");
        //var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\G6.csv");

        var loader = new CSVLoader.Loader();

        var _ = loader.ParseCSV(fi);

        return (loader.Invoices, loader.CustomerGroupToAccept);
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

    [Fact(Skip = "skip")]
    public async Task BookAll()
    {
        var invoiceDate = DateTime.Today;

        var csv = LoadCSV();
        var invoices = csv.Invoices;
        var customerGroupsToAccept = csv.CustomerGroupsToAccept;
        Assert.NotNull(invoices);
        Assert.True(invoices.Any());

        var gatewayInvoice = new GatewayBase(_settings, CustomerReport, new HttpClientHandler());
        var customerCache = await GetCustomerCache(gatewayInvoice, customerGroupsToAccept);
        var productCache = await GetProductCache(gatewayInvoice);

        var mapper = new Mapper(_settings, customerCache, productCache);

        foreach (var inputInvoice in invoices)
        {
            inputInvoice.InvoiceDate = invoiceDate; // TODO evaluate.

            var invoice = mapper.From(inputInvoice);

            if (invoice.Item2 is null)
            {
                Console.WriteLine($"Faulure on customer: {inputInvoice.CustomerNumber}");
                continue;
            }

            await gatewayInvoice.PushInvoice(invoice.Item1, invoice.Item2, inputInvoice.SourceFileLineNumber);
        }
    }
}
