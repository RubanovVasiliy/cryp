using System.Numerics;

namespace crypto;

public class Encryption
{
    private const int N = 1000000000;

    private static List<long> GenerateShamirKeys(int p)
    {
        var rnd = new Random();
        int c;
        List<long> gcd;
        do
        {
            c = rnd.Next(p - 1);
            gcd = CryptoLib.Gcd(p - 1, c);
        } while (!CryptoLib.IsSimple(c) && gcd[0] != 1);

        var d = gcd[2];
        if (d < 0)
        {
            d += p - 1;
        }

        return new List<long>() { c, d };
    }

    public static bool Shamir(long m)
    {
        var p = CryptoLib.GenerateSimpleNumber(N);
        var aShamirKeys = GenerateShamirKeys(p);
        var bShamirKeys = GenerateShamirKeys(p);


        var x1 = CryptoLib.Mod(m, aShamirKeys[0], p);
        var x2 = CryptoLib.Mod(x1, bShamirKeys[0], p);
        var x3 = CryptoLib.Mod(x2, aShamirKeys[1], p);
        var x4 = CryptoLib.Mod(x3, bShamirKeys[1], p);

        Console.WriteLine("{0} {1}", m, x4);

        return m == x4;
    }

    public static bool ElGamal(long m)
    {
        var n = 1000000;
        var p = CryptoLib.GenerateSimpleNumber(n, true);
        var q = (p - 1) / 2;
        long g;
        do
        {
            g = CryptoLib.GenerateSimpleNumber(p - 1);
        } while (CryptoLib.Mod(g, q, p) != 1);

        var ca = CryptoLib.GenerateSimpleNumber(p - 1);
        var da = CryptoLib.Mod(g, ca, p);
        
        var k = CryptoLib.GenerateSimpleNumber(p - 1);
        var r = CryptoLib.Mod(g, k, p);

        var cb = CryptoLib.GenerateSimpleNumber(p - 1);
        var db = CryptoLib.Mod(g, ca, p);
        
        var e = m * CryptoLib.Mod(db, k, p) % p;

        var m1 = e * CryptoLib.Mod(r, p - 1 - cb, p) % p;

        Console.WriteLine("{0} {1}", m, m1);

        return m == m1;
    }

    public static bool RSA(long m)
    {
        //var aRsaKeys = GenerateRSAKeys();
        var bRsaKeys = GenerateRSAKeys();

        var e = CryptoLib.Mod(m, bRsaKeys[1], bRsaKeys[2]);
        var m1 = CryptoLib.Mod(e, bRsaKeys[0], bRsaKeys[2]);
        
        return m == m1;
    }

    private static List<long> GenerateRSAKeys()
    {
        long n, d, c;
        do
        {
            long p = CryptoLib.GenerateSimpleNumber(N);
            long q = CryptoLib.GenerateSimpleNumber(N);
            n = p * q;
            long fi = (p - 1) * (q - 1);
            List<long> gcd;
            do
            {
                d = new Random().NextInt64(N);
                gcd = CryptoLib.Gcd(fi, d);
            } while (gcd[0] != 1 || d >= fi);

            c = gcd[2];
        } while (c < 1);

        return new List<long>() { c, d, n };
    }
}