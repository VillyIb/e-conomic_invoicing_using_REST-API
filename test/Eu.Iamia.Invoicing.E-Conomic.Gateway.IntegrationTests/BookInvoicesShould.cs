using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Invoicing.Loader.Contract;
using Eu.Iamia.Invoicing.Mapper;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class BookInvoicesShould
{

    private IList<IInputInvoice> LoadAllInvoices()
    {
        // Fakturering udeboende.csv
        // ØD Fakturaoverblik V2.csv
        var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\Fakturering udeboende.csv");

        var loader = new CSVLoader.Loader(fi);

        loader.ParseInvoiceFile();

        return loader.Invoices;
    }

    public async Task<CustomerCache> GetCustomerCache(GatewayBase gatewayInvoice)
    {

        var customerCache = new CustomerCache(gatewayInvoice);
        await customerCache.LoadAllCustomers();

        return customerCache;
    }
    public async Task<ProductCache> GetProductCache(GatewayBase gatewayInvoice)
    {

        var productCache = new ProductCache(gatewayInvoice);
        await productCache.LoadAllProducts();

        return productCache;
    }

    [Fact]
    public async Task BookAll()
    {
        var invoiceDate = DateTime.Today;

        var invoices = LoadAllInvoices();
        Assert.NotNull(invoices);
        Assert.True(invoices.Any());

        var gatewayInvoice = new GatewayBase(new HttpClientHandler());
        var customerCache = await GetCustomerCache(gatewayInvoice);
        var productCache = await GetProductCache(gatewayInvoice);

        var mapper = new Mapping(customerCache, productCache);

        foreach (var inputInvoice in invoices)
        {
            inputInvoice.InvoiceDate = invoiceDate; // TODO evaluate.

            var invoice = mapper.From(inputInvoice);

            if (invoice is null)
            {
                Console.WriteLine($"Faulure on customer: {inputInvoice.CustomerNumber}");
                continue;
            }

            //await gatewayInvoice.PushInvoice(invoice);
        }


    }
}
