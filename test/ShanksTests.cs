using System.Numerics;
using crypto;

namespace test;

public class ShanksTests
{
    [Fact]
    public void MeanNumbers()
    {
        var y = 9;
        var a = 2;
        var p = 23;

        var res = CryptoLib.Shanks(a, p, y);
        foreach (var i in res)
        {
            var actual = CryptoLib.ModPow(a, i, p);
            Assert.Equal(y, actual);
        }
    }

    [Fact]
    public void Random100Run()
    {
        for (var t = 0; t < 100; t++)
        {
            var n = 1000000000;
            var p = CryptoLib.GenerateSimpleNumber(n);
            
            var y = CryptoLib.GenerateSimpleNumber(Convert.ToInt32(p-1));
            var a = CryptoLib.GenerateSimpleNumber(Convert.ToInt32(p-1));

            var res = CryptoLib.Shanks(a, p, y);
            foreach (var i in res)
            {
                var actual = CryptoLib.ModPow(a, i, p);
                Assert.Equal(y, actual);
            }
        }
    }
}