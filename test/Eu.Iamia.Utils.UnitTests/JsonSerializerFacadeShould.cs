using System.Text.Json;

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
        var ex = Assert.Throws<JsonException>(() => JsonSerializerFacade.Deserialize<TestSubjectDto>((string)null!));
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

    // Asynchronous
    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    [Fact]
    public void DeserializeAsync_When_DtoThrowsArgumentException_Expect_JsonException()
    {
        var cts = new CancellationTokenSource();
        using var st = GenerateStreamFromString("{\"Subject\": \"\"}");

        var ex = Assert.ThrowsAsync<JsonException>(() => JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token));
        Assert.NotNull(ex.Result);
        Assert.NotNull(ex.Result.InnerException);
    }

    [Fact]
    public void DeserializeAsync_When_NullArgument_Expect_JsonException()
    {
        var cts = new CancellationTokenSource();
        using var st = GenerateStreamFromString((string)null);

        var ex = Assert.ThrowsAsync<JsonException>(() => JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token));
        Assert.NotNull(ex.Result);
        Assert.NotNull(ex.Result.InnerException);
    }

    [Fact]
    public void DeserializeAsync_ValidJson_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_ValidProperties()
    {
        var cts = new CancellationTokenSource();
        using var st = GenerateStreamFromString("{\"Subject\": \"x\"}");

        var dto = JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st,cts.Token);
        Assert.NotNull(dto.Result);
        Assert.NotNull(dto.Result.Subject);
    }

    [Fact]
    public void DeserializeAsync_ValidJson_WithoutProperties_When_JsonSerializerFacade_Deserialize_Returns_ValidDto_With_UninitializedProperties()
    {
        var cts = new CancellationTokenSource();
        using var st = GenerateStreamFromString("{}");

        var dto = JsonSerializerFacade.DeserializeAsync<TestSubjectDto>(st, cts.Token);
        Assert.NotNull(dto.Result);
        Assert.Null(dto.Result.Subject);
    }

}
