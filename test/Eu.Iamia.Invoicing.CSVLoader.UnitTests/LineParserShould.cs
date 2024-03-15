namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;
public  class LineParserShould
{
    [Theory]
    [InlineData(false, 0, "")]
    [InlineData(false, 0, ";;;;;;;;;;;;;;;;Konstanter;;;;;;;;;;")]
    [InlineData(false, 0, "Nr.;;Navn;Adresse;M2;Grundejer;Antal;Kontingent;Vand m3;Vand pris;Jordvarme kwh;Jordvarme pris;Nytteh m2;Nytteh pris;Total;;Nyttehave pr. m2;kr 1,00;;;;;;;;;")]
    [InlineData(true, 69, "69; ;Marianne Tholstrup;Månen  01;68;kr 408,00;1;kr 3.000,00;45;kr 2.525,85;;kr 0,00;110;kr 110,00;kr 6.043,85;;;;;;;;;;;;")]
    [InlineData(true, 93, "93; ;Villy Jørgensen og Kirsten Bjerritsgaard;Nordstjernen 09;178;kr 1.068,00;2;kr 6.000,00;75;kr 4.209,75;6;kr 1.440,00;;kr 0,00;kr 12.717,75;;;;;;;;;;;;")]
    [InlineData(false, 0, ";;Sum;;;kr 47.604,00;;kr 366.000,00;;kr 252.472,74;;kr 17.760,00;;kr 2.073,00;kr 685.909,74;;;;;;;;;;;;")]
    [InlineData(false, 0, ";;;;;;;;;;;;;;;;;;;;;;;;;;")]
    [InlineData(false, 0, "999;;;;;;;;;;;;;;;;;;;")] // too few columns.
    public void ParseLine(bool expectOk, int expecteCustomerNo, string line)
    {
        var sut = new LineParser(DateTime.Now);

        if (expectOk)
        {
            var actual = sut.FromString(line);
            Assert.NotNull(actual);
            Assert.NotNull(actual.CustomerNumber);
            Assert.Equal(expecteCustomerNo, actual.CustomerNumber.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => sut.FromString(line));
        }
    }

    [Theory]
    [InlineData(0, "93;;;;;;;;;;;;;;;;;;;;")]
    [InlineData(9, "93;;;;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21")]
    [InlineData(8, "93;;;;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;;")]
    [InlineData(7, "93;;;;4;5;6;7;8;9;10;11;12;13;14;15;16;17;;;;")]
    [InlineData(6, "93;;;;4;5;6;7;8;9;10;11;12;13;14;15;;;;;;")]
    [InlineData(5, "93;;;;4;5;6;7;8;9;10;11;12;13;;;;;;;;")]
    [InlineData(4, "93;;;;4;5;6;7;8;9;10;11;;;;;;;;;;")]
    [InlineData(3, "93;;;;4;5;6;7;8;9;;;;;;;;;;;;")]
    [InlineData(2, "93;;;;4;5;6;7;;;;;;;;;;;;;;")]
    [InlineData(1, "93;;;;4;5;;;;;;;;;;;;;;;;")]
    [InlineData(8, "93;;;;;;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21")]
    [InlineData(4, "93;;;;;;6;7;-8;9;-10;11;12;13;-14;15;16;17;-18;19;20;21")]
    [InlineData(3, "93;;;;0;5;0;7;0;9;0;11;0;13;0;15;16;17;18;19;20;21")]
    public void ParseLineWithAllProducts(int expectedLines, string line)
    {
        var sut = new LineParser(DateTime.Now);
        var actual = sut.FromString(line);
        Assert.NotNull(actual);
        Assert.Equal(expectedLines,actual.InvoiceLines.Count);
    }
}
