using System.Numerics;

namespace crypto;

public class Encryption
{
    private const int MaxValue = 1000000000;

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
        var p = CryptoLib.GenerateSimpleNumber(MaxValue);
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


    public class Rsa 
    {
        private readonly long _c;
        public long D { get; }
        public long N { get; }

        public Rsa()
        {
            var rsaKeys = GenerateRsaKeys();
            _c = rsaKeys[0];
            D = rsaKeys[1];
            N = rsaKeys[2];
        }


        public BigInteger Encrypt(long m, long d,long n)
        {
            return CryptoLib.Mod(m, d, n);
        }

        public long Decrypt(BigInteger m, long n)
        {
            return (long)CryptoLib.Mod(m, _c, n);
        }

        public void Encrypt(string filepath)
        {
            throw new NotImplementedException();
        }

        public void Decrypt(string filepath)
        {
            throw new NotImplementedException();
        }
        
        private static List<long> GenerateRsaKeys()
        {
            long n, d, c;
            do
            {
                long p = CryptoLib.GenerateSimpleNumber(MaxValue);
                long q = CryptoLib.GenerateSimpleNumber(MaxValue);
                n = p * q;
                long fi = (p - 1) * (q - 1);
                List<long> gcd;
                do
                {
                    d = new Random().NextInt64(MaxValue);
                    gcd = CryptoLib.Gcd(fi, d);
                } while (gcd[0] != 1 || d >= fi);

                c = gcd[2];
            } while (c < 1);

            return new List<long>() { c, d, n };
        }
    }


    public class Xor : IEncryptable
    {
        private readonly int _code = new Random().Next(255);

        public long Encrypt(long m)
        {
            return m ^ _code;
        }

        public long Decrypt(long m)
        {
            return m ^ _code;
        }

        public void Encrypt(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException();
            }

            using var fileStream = new FileStream(filepath, FileMode.Open);
            var buffer = new byte[fileStream.Length];

            if (fileStream.Read(buffer) != fileStream.Length)
            {
                throw new FileLoadException();
            }

            fileStream.Close();

            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)_code;
            }

            var encryptedFileName = "XorEnc_" + Path.GetFileName(fileStream.Name);
            using var binWriter = new BinaryWriter(File.Open(encryptedFileName, FileMode.Create));

            binWriter.Write(buffer);
            binWriter.Close();
        }

        public void Decrypt(string filepath)
        {
            Encrypt(filepath);
        }
    }
}