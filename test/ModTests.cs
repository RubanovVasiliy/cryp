using System.Numerics;
using crypto;

namespace test;

public class ModTests
{
    [Fact]
    public void SmallNumbers()
    {
        var a = 3;
        var x = 100;
        var p = 7;

        var actual = CryptoLib.ModPow(a, x, p);
        var expected = BigInteger.ModPow(a, x, p);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MeanNumbers()
    {
        var a = 12345678;
        var x = 83475674;
        var p = 27529835;

        var actual = CryptoLib.ModPow(a, x, p);
        var expected = BigInteger.ModPow(a, x, p);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RandomNumbers10Run()
    {
        for (int i = 0; i < 10; i++)
        {
            var a = new Random().Next();
            var x = new Random().Next();
            var p = new Random().Next();

            var actual = CryptoLib.ModPow(a, x, p);
            var expected = BigInteger.ModPow(a, x, p);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void ZeroDegree()
    {
        var a = 2;
        var x = 0;
        var p = 3;

        var actual = CryptoLib.ModPow(a, x, p);
        var expected = BigInteger.ModPow(a, x, p);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ZeroA()
    {
        var a = 0;
        var x = 53457;
        var p = 35638;

        var actual = CryptoLib.ModPow(a, x, p);
        var expected = BigInteger.ModPow(a, x, p);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ZeroP()
    {
        var a = 72678;
        var x = 5457;
        var p = 0;

        var actual = "P can not be <= 0";
        try
        {
            CryptoLib.ModPow(a, x, p);
        }
        catch (Exception e)
        {
            Assert.Equal(actual,e.Message);
        }
    }

    [Fact]
    public void ZeroAll()
    {
        var a = 0;
        var x = 0;
        var p = 0;

        var actual = "P can not be 0";
        try
        {
             CryptoLib.ModPow(a, x, p);
        }
        catch (Exception e)
        {
            Assert.Equal(actual,e.Message);
        }
    }
}