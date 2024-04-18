using System.Runtime.Serialization;

namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;
public class LoaderShould
{

    // TODO this is an integration test.
    [Fact(Skip ="move to integration")]
    public void LoadFileOk()
    {
        var fi = new FileInfo("C:\\Development\\e-conomic_invoicing_using_REST-API\\test\\Eu.Iamia.Invoicing.CSVLoader.UnitTests\\TestData\\G2.csv");

        var loader = new Loader();

        loader.ParseCSV(fi);

        Assert.True(loader.Invoices.Any());

    }
}
