using System.Numerics;
using System.Security.Cryptography;
using crypto;

namespace lab5;

public class BlindSignature
{
    public static void s()
    {
        BigInteger p, q, n, c, d, fi;

        do
        {
            p = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(1024);
        } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(p));

        do
        {
            q = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(1024);
        } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(q) || p == q);

        n = p * q;
        fi = (p - 1) * (q - 1);

        List<BigInteger> gcd;
        do
        {
            c = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
            gcd = CryptoLib.Gcd(fi, c);
            d = gcd[2];
        } while (gcd[0] != 1 || d < 0);

    }

    public static BigInteger alice(BigInteger d, BigInteger N, int vote = 1)
    {
        BigInteger r;
        List<BigInteger> gcd;
        do
        {
            r = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
            gcd = CryptoLib.Gcd(N, r);
        } while (gcd[0] != 1);

        var rnd = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(512);
        var n = BigInteger.Parse(rnd.ToString() + vote);
        var h = SHA512.HashData(n.ToByteArray());

        var h1 = new BigInteger(h) * CryptoLib.ModPow(r, d, n) % n;

        return h1;
    }

}