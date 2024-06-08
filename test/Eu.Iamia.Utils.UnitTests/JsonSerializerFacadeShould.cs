using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eu.Iamia.Utils.UnitTests;

public class JsonSerializerFacadeShould
{
    // Synchronous

    [Fact]
    public void Deserialize_When_DtoThrowsArgumentException_Expect_JsonException()
    {
        var ex = Assert.Throws<JsonException>(() => JsonSerializerFacade.Deserialize<TestSubjectDto>("{\"Subject\": \"\"}"));
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public void Deserialize_When_NullArgument_Expect_JsonException()
    {
        var ex = Assert.Throws<JsonException>(() => JsonSerializerFacade.Deserialize<TestSubjectDto>(null!));
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public void Given_ValidJson_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_ValidProperties()
    {
        var dto = JsonSerializerFacade.Deserialize<TestSubjectDto>("{\"Subject\": \"x\"}");
        Assert.NotNull(dto);
        Assert.NotNull(dto.Subject);
    }

    [Fact]
    public void Given_ValidJson_WithoutProperties_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_UninitializedProperties()
    {
        var dto = JsonSerializerFacade.Deserialize<TestSubjectDto>("{}");
        Assert.NotNull(dto);
        Assert.Null(dto.Subject);
    }

    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters = { new JsonStringEnumConverter() }
        ,
        MaxDepth = 0
        ,
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower
        ,
        ReadCommentHandling = JsonCommentHandling.Skip
        ,
        AllowTrailingCommas = true
        ,
        NumberHandling = JsonNumberHandling.Strict
        ,
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement
        ,
        WriteIndented = true

        //, ReferenceHandler = ReferenceHandler.Preserve
        //, TypeInfoResolver = new DefaultJsonTypeInfoResolver(){  Modifiers = new List<Action<JsonTypeInfo>>(){ }}
    };

    [Fact]
    public void Deserialize_X()
    {
        var expected = new DeepTestSubjectL0Dto
        {
            L1 = new DeepTestSubjectL1Dto
            {
                L2 = new DeepTestSubjectL2Dto
                {
                    L3 = new DeepTestSubjectL3Dto
                    {
                        L4 = "Level4"
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(expected, Options);


        var dto = JsonSerializerFacade.Deserialize<TestSubjectDto>(json);
        Assert.NotNull(dto);
        Assert.Null(dto.Subject);
    }

    // Asynchronous
    private static Stream GenerateStreamFromString(string? s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    [Fact]
    public async Task DeserializeAsync_When_DtoThrowsArgumentException_Expect_JsonException()
    {
        var cts = new CancellationTokenSource();
        await using var st = GenerateStreamFromString("{\"Subject\": \"\"}");

        var ex = await Assert.ThrowsAsync<JsonException>(() => JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token));
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public async Task DeserializeAsync_When_NullArgument_Expect_JsonException()
    {
        var cts = new CancellationTokenSource();
        await using var st = GenerateStreamFromString(null);

        var ex = await Assert.ThrowsAsync<JsonException>(() => JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token));
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public async Task DeserializeAsync_ValidJson_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_ValidProperties()
    {
        var cts = new CancellationTokenSource();
        await using var st = GenerateStreamFromString("{\"Subject\": \"x\"}");

        var dto = await JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Subject);
    }

    [Fact]
    public async Task DeserializeAsync_ValidJson_WithoutProperties_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_UninitializedProperties()
    {
        var cts = new CancellationTokenSource();
        await using var st = GenerateStreamFromString("{}");

        var dto = await JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token);
        Assert.NotNull(dto);
        Assert.Null(dto.Subject);
    }

}
