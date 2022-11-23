using System.Numerics;
using crypto;

namespace graph_coloring;

public class Node
{
    public Node(int id)
    {
        Id = id;
        BigInteger q;
        BigInteger p;
        do
        {
            p = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(512);
        } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(p));

        do
        {
            q = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(512);
        } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(q) || p == q);

        N = p * q;
        var fi = (p - 1) * (q - 1);

        List<BigInteger> gcd;
        do
        {
            C = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
            gcd = CryptoLib.Gcd(fi, C);
            D = gcd[2];
        } while (gcd[0] != 1 || D < 0);
    }

    public BigInteger CreateZ(string colors)
    {
        var rnd = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(512);
        var ch = new byte[] { Convert.ToByte(colors.ToCharArray()[Id - 1]) };
        R = new BigInteger(rnd.ToByteArray().Concat(ch).ToArray());
        Z = BigInteger.ModPow(R, D, N);
        return Z;
    }
    

    public int Id { get; }
    public BigInteger D { get; }
    public BigInteger C { get; }
    public BigInteger N { get; }
    public BigInteger R { get; set; }
    public BigInteger Z { get; set; }

}