namespace Eu.Iamia.Utils.UnitTests;

public class StringExtensionsShould
{
    [Theory]
    [InlineData("", 0, "AbCdEf")]
    [InlineData("A", 1, "AbCdEf")]
    [InlineData("Ab", 2, "AbCdEf")]
    [InlineData("AbC", 3, "AbCdEf")]
    [InlineData("AbCd", 4, "AbCdEf")]
    [InlineData("AbCdE", 5, "AbCdEf")]
    [InlineData("AbCdEf", 6, "AbCdEf")]
    [InlineData("AbCdEf@", 7, "AbCdEf")]
    [InlineData("AbCdEf@@", 8, "AbCdEf")]
    [InlineData("@@@@", 4, "")]
    [InlineData("@@@@", 4, null)]
    public void TrimToLength_Returns_RightValue(string expected, int length, string? input)
    {
        const char postfix = '@';
        var actual = input.TrimToLength(length, postfix);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", 0, "AbCdEf")]
    [InlineData("A", 1, "AbCdEf")]
    [InlineData("Ab", 2, "AbCdEf")]
    [InlineData("AbC", 3, "AbCdEf")]
    [InlineData("AbCd", 4, "AbCdEf")]
    [InlineData("AbCdE", 5, "AbCdEf")]
    [InlineData("AbCdEf", 6, "AbCdEf")]
    [InlineData("@AbCdEf", 7, "AbCdEf")]
    [InlineData("@@AbCdEf", 8, "AbCdEf")]
    [InlineData("@@@@", 4, "")]
    [InlineData("@@@@", 4, null)]
    public void TrimToNumberLength_Returns_RightValue(string expected, int length, string? input)
    {
        const char postfix = '@';
        var actual = input.TrimNumberToLength(length, postfix);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetStream_FromString_ReadToEnd_ReturnsSameString()
    {
        const string mockString = "øæåØÆÅ$£@€ #tags;#DebitorNummer;;;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;;\r\n#Kundegrupper;1,3;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;ØD fakturering for 2024;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;;;;;;;;;;;;;;;;;;;;;;\r\n#Bilagsdato;2023-03-24;;<- anføres som YYYY-MM-DD;;;;;;;;;;;;;;;;;;;\r\n#Betalingsbetingelse;1;;<- bb-nummer i e-conomic;;;;;;;;;;;;;;;;;;;\r\n#Produkt;;;Varenummer i e-conomic NB! string prefix with ';;;3;;;27;;;4;;;24;;;25;;;;";

        using var stream = mockString.GetStream();

        using var streamReader = new StreamReader(stream);

        var actual = streamReader.ReadToEnd();

        Assert.True(mockString.Equals(actual));
    }
}