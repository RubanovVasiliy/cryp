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
        var actual = CryptoLib.Mod(2,res,23);
        Assert.Equal(y, actual);
    }
}