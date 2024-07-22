namespace Eu.Iamia.Utils.UnitTests;

[NCrunch.Framework.Category("Unit")]
public class IntervalShould
{
    [Theory]
    [InlineData(true, "2024-01-01", "2024-12-31")]
    [InlineData(true, "2024-01-01", "2024-01-01")]
    [InlineData(false, "2024-12-31", "2024-01-01")]
    public void Create_Type_DateTime(bool expectSuccess, string from, string to)
    {
        var fd = DateTime.Parse(from);
        var td = DateTime.Parse(to);

        if (expectSuccess)
        {
            var _ = Interval<DateTime>.Create(fd, td);
            Assert.Equal(fd, _.From);
            Assert.Equal(td, _.To);
        }
        else
        {
            var msg = Assert.Throws<ArgumentException>(() => Interval<DateTime>.Create(fd, td));
        }
    }

    [Theory]
    [InlineData(true, 10.0, 20.0)]
    [InlineData(true, 10.0, 10.0)]
    [InlineData(false, 20.0, 10.0)]
    public void Create_Type_float(bool expectSuccess, float from, float to)
    {
        if (expectSuccess)
        {
            var _ = Interval<float>.Create(from, to);
            Assert.Equal(from, _.From);
            Assert.Equal(to, _.To);
        }
        else
        {
            var msg = Assert.Throws<ArgumentException>(() => Interval<float>.Create(from, to));
        }
    }
}
