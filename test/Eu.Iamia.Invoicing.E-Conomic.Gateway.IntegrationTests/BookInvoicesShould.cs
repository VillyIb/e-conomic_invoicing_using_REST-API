using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class BookInvoicesShould
{

    private IList<IInputInvoice> LoadAllInvoices()
    {
        var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\ØD Fakturaoverblik V2.csv");

        var loader = new CSVLoader.Loader(fi);

        loader.ParseInvoiceFile();

        return loader.Invoices;
    }

    public async Task<CustomerCache> x(GatewayBase gatewayInvoice)
    {

        var customerCache = new CustomerCache(gatewayInvoice);
        await customerCache.LoadAllCustomers();

        return customerCache;
    }

    [Fact]
    public async Task BookAll()
    {
        var invoices = LoadAllInvoices();
        Assert.NotNull(invoices);
        Assert.True(invoices.Any());

        var gatewayInvoice = new GatewayBase();
        var customerCache = await x(gatewayInvoice);

    }
}
