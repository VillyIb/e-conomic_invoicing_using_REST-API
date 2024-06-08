using System.Text.Json;

namespace Eu.Iamia.Utils.UnitTests;

/// <summary>
/// 
/// </summary>
/// <exception cref="ArgumentNullException"></exception>
public class JsonSerializerFacadeV2Stub1 : JsonSerializerFacadeV2
{
    protected override TValue? Deserialize<TValue>(string json, JsonSerializerOptions options) where TValue : default
    {
        throw new ArgumentNullException(nameof(json));
    }

    protected override ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonSerializerOptions options, CancellationToken cancellationToken) where TValue : default
    {
        throw new ArgumentNullException(nameof(utf8Json));
    }
}

/// <summary>
/// 
/// </summary>
/// <exception cref="JsonException"></exception>
public class JsonSerializerFacadeV2Stub2 : JsonSerializerFacadeV2
{
    protected override TValue? Deserialize<TValue>(string json, JsonSerializerOptions options) where TValue : default
    {
        throw new JsonException();
    }

    protected override ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonSerializerOptions options, CancellationToken cancellationToken) where TValue : default
    {
        throw new JsonException();
    }
}

/// <summary>
/// Returns default TValue.
/// </summary>
public class JsonSerializerFacadeV2Stub3 : JsonSerializerFacadeV2
{
    protected override TValue? Deserialize<TValue>(string json, JsonSerializerOptions options) where TValue : default
    {
        return default;
    }

    protected override ValueTask<TValue?> DeserializeAsync<TValue>(Stream utf8Json, JsonSerializerOptions options, CancellationToken cancellationToken) where TValue : default
    {
        return new ValueTask<TValue?>();
    }
}

public class JsonSerializerFacadeV2Should
{
    // sync 

    [Fact]
    public void Deserialize_Throws_ArgumentNullException_HandledOK()
    {
        var sut = new JsonSerializerFacadeV2Stub1();
        var _ = Assert.Throws<JsonException>(() => sut.Deserialize<TestSubjectDto>(null!));
    }

    [Fact]
    public void Deserialize_Throws_JsonException_HandledOK()
    {
        var sut = new JsonSerializerFacadeV2Stub2();
        var _ = Assert.Throws<JsonException>(() => sut.Deserialize<TestSubjectDto>(null!));
    }

    [Fact]
    public void Deserialize_Returns_Null_HandledOK()
    {
        var sut = new JsonSerializerFacadeV2Stub3();
        var _ = Assert.Throws<JsonException>(() => sut.Deserialize<TestSubjectDto>(null!));
    }

    [Fact]
    public void Deserialize_ValidJson_Returns_ExpectedValue()
    {
        var expected = new TestSubjectDto { Subject = "subject" };
        var sut = new JsonSerializerFacadeV2();
        var actual = sut.Deserialize<TestSubjectDto>("{\"Subject\":\"subject\"}");
        Assert.Equal(expected, actual);
    }

    // async 

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
    public async Task DeserializeAsync_Throws_ArgumentNullException_HandledOK()
    {
        await using var st = GenerateStreamFromString("{\"Subject\":\"subject\"}");

        var sut = new JsonSerializerFacadeV2Stub1();
        var _ = await Assert.ThrowsAsync<JsonException>(() => sut.DeserializeAsync<TestSubjectDto>(st));
    }

    [Fact]
    public async Task DeserializeAsync_Throws_JsonException_HandledOK()
    {
        await using var st = GenerateStreamFromString("{\"Subject\":\"subject\"}");

        var sut = new JsonSerializerFacadeV2Stub2();
        var _ = await Assert.ThrowsAsync<JsonException>(() => sut.DeserializeAsync<TestSubjectDto>(st));
    }

    [Fact]
    public async Task DeserializeAsync_Returns_Null_HandledOK()
    {
        await using var st = GenerateStreamFromString("{\"Subject\":\"subject\"}");

        var sut = new JsonSerializerFacadeV2Stub3();
        var _ = await Assert.ThrowsAsync<JsonException>(() => sut.DeserializeAsync<TestSubjectDto>(st));
    }

    [Fact]
    public async Task DeserializeAsync_ValidJson_Returns_ExpectedValue()
    {
        var cts = new CancellationTokenSource();
        await using var st = GenerateStreamFromString("{\"Subject\":\"subject\"}");

        var expected = new TestSubjectDto { Subject = "subject" };
        var sut = new JsonSerializerFacadeV2();
        var actual = await sut.DeserializeAsync<TestSubjectDto>(st, cts.Token);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

}
