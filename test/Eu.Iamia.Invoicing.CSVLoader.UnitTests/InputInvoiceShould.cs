namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;

public class InputInvoiceShould
{
    [Fact]
    public void PersistLines()
    {
        var sut= new InputInvoice();
        sut.InvoiceLines.Add(new InputLine());
        Assert.Equal(1, sut.InvoiceLines.Count);
        sut.InvoiceLines.Add(new InputLine());
        Assert.Equal(2, sut.InvoiceLines.Count);
    }
}