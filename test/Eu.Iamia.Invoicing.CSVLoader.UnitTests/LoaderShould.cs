namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;
public class LoaderShould
{

    [Fact]
    public void LoadFileOk()
    {
        var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\ØD Fakturaoverblik V2.csv");

        var loader = new Loader(fi);

        loader.ParseCSV();

        Assert.True(loader.Invoices.Any());

    }
}
