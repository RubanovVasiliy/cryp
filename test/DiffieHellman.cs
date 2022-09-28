using System.Numerics;
using crypto;

namespace test;

public class DiffieHellman
{
    [Fact]
    public void RandomRun()
    {
        for (var i = 0; i < 1000; i++)
        {
            Assert.True(CryptoLib.Diffie());
        }
    }
}