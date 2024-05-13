using System.Text.Json;

namespace Eu.Iamia.Utils.UnitTests;

public class JsonSerializerFacadeShould
{

    [Fact]
    public void Given_DtoThrowsArgumentException_When_JsonSerializerFacade_Deserialize_ThrowsJsonExceptionWithInnerException()
    {
        var ex = Assert.Throws<JsonException>(() => JsonSerializerFacade.Deserialize<TestSubjectDto>("{\"Subject\": \"\"}"));
        Assert.NotNull(ex);
        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public void Given_NullArgument_When_JsonSerializerFacade_Deserialize_ThrowsJsonExceptionWithInnerException()
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
}
