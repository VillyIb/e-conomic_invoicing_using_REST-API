using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable ConvertToAutoProperty

namespace Eu.Iamia.Utils.UnitTests.InvestigateJsonSerializer;

/// <summary>
/// Investigate behaviour of System.Text.Json.JsonSerializer.Deserialize.
/// </summary>
public class JsonSerializerShould
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters = { new JsonStringEnumConverter() }
        ,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    [Fact]
    public void Given_DtoThrowsArgumentException_When_JsonSerializer_Deserialize_IsTransparent()
    {
        // ReSharper disable once InconsistentNaming
        var json_TestSubjectDtoThrowsArgumentException = "{\"Subject\": \"\"}";
        var ex = Assert.Throws<ArgumentException>(() => JsonSerializer.Deserialize<TestSubjectDto>(json_TestSubjectDtoThrowsArgumentException, _options));
        Assert.NotNull(ex);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void Given_NullArgument_When_JsonSerializer_Deserialize_Throws_ArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => JsonSerializer.Deserialize<TestSubjectDto>((string)null!, _options));
        Assert.NotNull(ex);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void Given_ValidJson_When_JsonSerializer_Deserialize_Returns_ValidDto_With_ValidProperties()
    {
        var dto = JsonSerializer.Deserialize<TestSubjectDto>("{\"Subject\": \"x\"}", _options);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Subject);
    }

    [Fact]
    public void Given_ValidJson_WithoutProperties_When_JsonSerializer_Deserialize_Returns_ValidDto_With_UninitializedProperties()
    {
        var dto = JsonSerializer.Deserialize<TestSubjectDto>("{}", _options);
        Assert.NotNull(dto);
        Assert.Null(dto.Subject);
    }
}