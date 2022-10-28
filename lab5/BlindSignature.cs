using System.Numerics;
using crypto;

namespace lab5;

public class BlindSignature
{
    public static void s()
    {
        BigInteger p, q,n,c,d;

        do
        {
            p = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
            q = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
        } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(p) || 
                 !CryptoLib.PrimeExtensions.IsProbablyPrime(q) ||
                 p == q);
        
        n = (p - 1) * (q - 1);
        List<BigInteger> gcd;

        do
        {
            c = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
            gcd = CryptoLib.Gcd(n, c);
            d = gcd[2];
        } while (gcd[0] != 1 || d < 0 );

        Console.WriteLine(p);
        Console.WriteLine(q);
        Console.WriteLine(n);
        Console.WriteLine(c);
        Console.WriteLine(d);

    }
}