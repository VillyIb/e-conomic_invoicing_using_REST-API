namespace Eu.Iamia.Utils.UnitTests;

public class CharComparerExact : IEqualityComparer<char>
{
    public bool Equals(char x, char y)
    {
        return x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(char obj)
    {
        return obj.GetHashCode();
    }
}

public class StringExtensionsShould
{
    private static bool RightLengthOfResult(int length, string actual)
    {
        return length == actual.Length;
    }

    private static bool RightLengthFromOriginalContent(int affix, string? input, int length, string actual)
    {
        var charComparer = new CharComparerExact();
        return (length - affix) == actual.Intersect(input ?? string.Empty, charComparer ).Count();
    }

    private static bool RightNumberOfAffixCharacters(int expectedPostfixCount, string actual, char affixChar)
    {
        return expectedPostfixCount == actual.Count(ch => ch == affixChar);
    }

    [Theory]
    [InlineData(0, "1234", 0)]
    [InlineData(0, "1234", 1)]
    [InlineData(0, "1234", 2)]
    [InlineData(0, "1234", 3)]
    [InlineData(0, "1234", 4)]
    [InlineData(1, "1234", 5)]
    [InlineData(4, "", 4)]
    [InlineData(4, null, 4)]
    [InlineData(0, "aAæÆøØåÅ" , 8)]
    public void TrimToLength_Returns_ExpectedLength_With_VoidPostfixFilled(int expectedPostfixCount, string? input, int length)
    {
        const char postfix = '@';

        var actual = input.TrimToLength(length, postfix);

        Assert.True(RightLengthOfResult(length, actual));
        Assert.True(RightLengthFromOriginalContent(expectedPostfixCount, input, length, actual));
        Assert.True(RightNumberOfAffixCharacters(expectedPostfixCount, actual, postfix));
    }

    [Theory]
    [InlineData(0, "1234", 0)]
    [InlineData(0, "1234", 1)]
    [InlineData(0, "1234", 2)]
    [InlineData(0, "1234", 3)]
    [InlineData(0, "1234", 4)]
    [InlineData(1, "1234", 5)]
    [InlineData(4, "", 4)]
    [InlineData(4, null, 4)]
    [InlineData(0, "aAæÆøØåÅ", 8)]
    public void TrimNumberToLength_Returns_ExpectedLength_With_VoidPrefixFilled(int expectedPrefixCount, string? input, int length)
    {
        const char prefix = '@';

        var actual = input.TrimNumberToLength(length, prefix);

        Assert.True(RightLengthOfResult(length, actual));
        Assert.True(RightLengthFromOriginalContent(expectedPrefixCount, input, length, actual));
        Assert.True(RightNumberOfAffixCharacters(expectedPrefixCount, actual, prefix));
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