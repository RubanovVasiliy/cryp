using System.Numerics;
using crypto;

namespace test;

public class GcdTests
{
    [Fact]
    public void SmallNumbers()
    {
        var a = 28;
        var b = 19;

        var actual = CryptoLib.Gcd(a, b);
        var expected = new List<long> { 1, -2, 3 };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BigNumbers()
    {
        var a = 1456660041;
        var b = 770514635;

        var actual = CryptoLib.Gcd(a, b);
        var expected = new List<long> { 1, 201786476, -381477889 };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ZeroB()
    {
        var a = 770514635;
        var b = 0;

        var actual = CryptoLib.Gcd(a, b);
        var expected = new List<long> { 770514635, 1, 0 };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ALowerThenB()
    {
        var a = 0;
        var b = 125;

        var actual = "In GCD must be a >= b";
        try
        {
            CryptoLib.Gcd(a, b);
        }
        catch (Exception e)
        {
            Assert.Equal(actual, e.Message);
        }
    }
}