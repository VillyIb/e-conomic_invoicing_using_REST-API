namespace Eu.Iamia.Utils.UnitTests
{
    public class CheckForNullShould
    {
        [Theory]
        [InlineData(false, "Xyz", "Xyz")]
        [InlineData(false, 0, 0)]
        [InlineData(false, 1.1, 1.1)]
        [InlineData(false, 2L, 2L)]
        [InlineData(false, 3.3D, 3.3D)]
        [InlineData(true, "input", null)]
        public void VerifyCheckForNull(bool expectError, object expected, object? input)
        {
            if (expectError)
            {
                Exception ex = Assert.Throws<NullReferenceException>(() => input.CheckForNull(nameof(input)));
                Assert.Equal(expected, ex.Message);
            }
            else
            {
                object actual = input.CheckForNull(nameof(input));
                Assert.Equal(expected, actual);
            }
        }
    }
}
