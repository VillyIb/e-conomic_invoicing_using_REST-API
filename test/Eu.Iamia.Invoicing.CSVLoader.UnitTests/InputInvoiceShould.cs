namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;

public class InputInvoiceShould
{
    [Fact]
    public void PersistLines()
    {
        var sut= new InputInvoice();
        sut.InvoiceLines.Add(new InputLine());
        Assert.Single(sut.InvoiceLines);
        sut.InvoiceLines.Add(new InputLine());
        Assert.Equal(2, sut.InvoiceLines.Count);
    }
}