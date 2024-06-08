// ReSharper disable StringLiteralTypo
namespace Eu.Iamia.Utils.UnitTests;

public class StringExtensionsShould
{
    [Fact]
    public void JsonPrettify_For_Valid_Json_Returns_Formatted_Json()
    {
        const string source = "{ \"Alfa\": \"aaaa\", \"Bravo\": \"bbbb\"}";
        const string expected = "{\r\n  \"Alfa\": \"aaaa\",\r\n  \"Bravo\": \"bbbb\"\r\n}";
        var actual = source.JsonPrettify();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void JsonPrettify_For_Invalid_Json_Returns_Same()
    {
        const string source = "{ \"Alfa\" \"aaaa\", \"Bravo\" \"bbbb\"}";
        const string expected = source;
        var actual = source.JsonPrettify();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", '@', 0, "AbCdEf")]
    [InlineData("A", '@', 1, "AbCdEf")]
    [InlineData("Ab", '@', 2, "AbCdEf")]
    [InlineData("AbC", '@', 3, "AbCdEf")]
    [InlineData("AbCd", '@', 4, "AbCdEf")]
    [InlineData("AbCdE", '@', 5, "AbCdEf")]
    [InlineData("AbCdEf", '@', 6, "AbCdEf")]
    [InlineData("AbCdEf@", '@', 7, "AbCdEf")]
    [InlineData("AbCdEf@@", '@', 8, "AbCdEf")]
    [InlineData("@@@@", '@', 4, "")]
    [InlineData("…………", null, 4, "")]
    [InlineData("zzzz", 'z', 4, null)]
    public void TrimToLength_Returns_RightValue(string expected, char? postfix, int length, string? input)
    {
        var actual = postfix.HasValue
            ? input.TrimToLength(length, postfix.Value)
            : input.TrimToLength(length)
        ;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", '@', 0, "AbCdEf")]
    [InlineData("A", '@', 1, "AbCdEf")]
    [InlineData("Ab", '@', 2, "AbCdEf")]
    [InlineData("AbC", '@', 3, "AbCdEf")]
    [InlineData("AbCd", '@', 4, "AbCdEf")]
    [InlineData("AbCdE", '@', 5, "AbCdEf")]
    [InlineData("AbCdEf", '@', 6, "AbCdEf")]
    [InlineData("@AbCdEf", '@', 7, "AbCdEf")]
    [InlineData("@@AbCdEf", '@', 8, "AbCdEf")]
    [InlineData("@@@@", '@', 4, "")]
    [InlineData("____", null, 4, "")]
    [InlineData("zzzz", 'z', 4, null)]
    public void TrimToNumberLength_Returns_RightValue(string expected, char? postfix, int length, string? input)
    {
        var actual = postfix.HasValue
                ? input.TrimNumberToLength(length, postfix.Value)
                : input.TrimNumberToLength(length)
            ;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetStream_FromString_ReadToEnd_ReturnsSameString()
    {
        const string mockString = "øæåØÆÅ$£@€ #tags;#DebitorNummer;;;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;#Produkt;;;;\r\n#Kundegrupper;1,3;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;ØD fakturering for 2024;;;;;;;;;;;;;;;;;;;;;\r\n#Tekst1;;;;;;;;;;;;;;;;;;;;;;\r\n#Bilagsdato;2023-03-24;;<- anføres som YYYY-MM-DD;;;;;;;;;;;;;;;;;;;\r\n#Betalingsbetingelse;1;;<- bb-nummer i e-conomic;;;;;;;;;;;;;;;;;;;\r\n#Produkt;;;Varenummer i e-conomic NB! string prefix with ';;;3;;;27;;;4;;;24;;;25;;;;";

        using var stream = mockString.GetStream();

        using var streamReader = new StreamReader(stream);

        var actual = streamReader.ReadToEnd();

        Assert.Equal(mockString, actual);
    }


}