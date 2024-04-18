namespace Eu.Iamia.Utils.UnitTests;

public class StringExtensionsShould
{
    [Theory]
    [InlineData("1234", "1234")]
    [InlineData("123x", "123")]
    [InlineData("12xx", "12")]
    [InlineData("1xxx", "1")]
    [InlineData("xxxx", "")]
    [InlineData("xxxx", null)]
    [InlineData("1234", "12345")]
    public void Given_InputOfSpecificLenght_When_TrimToLength_ExpectOutput(string expected, string input)
    {
        var actual = input.TrimToLength(4, 'x');
        Assert.Equal(expected, actual);
    }


    [Theory]
    [InlineData("1234", "1234")]
    [InlineData("_123", "123")]
    [InlineData("__12", "12")]
    [InlineData("___1", "1")]
    [InlineData("____", "")]
    [InlineData("____", null)]
    [InlineData("1234", "12345")]
    public void Given_InputOfSpecificLenght_When_TrimNumberLoLength_ExpectOutput(string expected, string input)
    {
        var actual = input.TrimNumberToLength(4, '_');
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void     Given_SpecificString_When_GetAsStream_OutputReturnsSpecificString()
    {
         const string alfa = "øæåØÆÅ$£@€ #tags;#DebitorNummer;;;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;;\r\n#Kundegrupper;1,3;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;ØD fakturering for 2024;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;;;;;;;;;;;;;;;;;;;;;;\r\n#Bilagsdato;2023-03-24;;<- anføres som YYYY-MM-DD;;;;;;;;;;;;;;;;;;;\r\n#Betalingsbetingelse;1;;<- bb-nummer i e-conomic;;;;;;;;;;;;;;;;;;;\r\n#Produkt;;;Varenummer i e-conomic NB! string prefix with ';;;3;;;27;;;4;;;24;;;25;;;;";

        using var st = alfa.GetStream();

        using var sr = new StreamReader(st);

        var actual = sr.ReadToEnd();

        Assert.Equal(alfa, actual);
    }
}