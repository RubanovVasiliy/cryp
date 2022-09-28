using System.Numerics;
using crypto;

namespace test;

public class DiffieHellman
{
    [Fact]
    public void Random10Run()
    {
        for (var i = 0; i < 10; i++)
        {
            Assert.True(CryptoLib.DiffieHellman());
        }
    }
}