namespace crypto;

public class Encryption
{
    private const int MaxValue = 1000000000;

    public static bool ElGamal(long m)
    {
        var n = 1000000;
        var p = CryptoLib.GenerateSimpleNumber(n, true);
        var q = (p - 1) / 2;
        long g;
        do
        {
            g = CryptoLib.GenerateSimpleNumber(p - 1);
        } while (CryptoLib.ModPow(g, q, p) != 1);

        var ca = CryptoLib.GenerateSimpleNumber(p - 1);
        var da = CryptoLib.ModPow(g, ca, p);

        var k = CryptoLib.GenerateSimpleNumber(p - 1);
        var r = CryptoLib.ModPow(g, k, p);

        var cb = CryptoLib.GenerateSimpleNumber(p - 1);
        var db = CryptoLib.ModPow(g, ca, p);

        var e = m * CryptoLib.ModPow(db, k, p) % p;

        var m1 = e * CryptoLib.ModPow(r, p - 1 - cb, p) % p;

        Console.WriteLine("{0} {1}", m, m1);

        return m == m1;
    }

    public class Shamir
    {
        public Shamir(int p)
        {
            var shamirKeys = GenerateShamirKeys(p);
            P = p;
            C = shamirKeys[0];
            D = shamirKeys[1];
        }
        public int P { get; }
        public long D { get; }
        public long C { get; }
        
        
        public long Encrypt(long m)
        {
            return CryptoLib.ModPow(m, C, P);
        }

        public long Decrypt(long m)
        {
            return  CryptoLib.ModPow(m, D, P);
        }

        public void Encrypt(string filepath, long c,long d)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var encrypted = new List<long>();
            foreach (var item in buffer)
            {
                var x1 = CryptoLib.ModPow(item, C, P);
                var x2 = CryptoLib.ModPow(x1, c, P);
                encrypted.Add(x2);
            }

            const string prefix = "ShamirEnc_";
            var encryptedFileName = prefix + Path.GetFileName(filepath);
            using var binWriter = new BinaryWriter(File.Open(encryptedFileName, FileMode.Create));
            foreach (long item in encrypted)
            {
                binWriter.Write(item);
            }

            binWriter.Close();
        }

        public void Decrypt(string filepath, long c,long d)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var decrypted = new List<byte>();
            for (var i = 0; i < buffer.Length; i += 8)
            {
                var value = BitConverter.ToInt64(buffer, i);
                var x3 = CryptoLib.ModPow(value, D, P);
                var x4 = CryptoLib.ModPow(x3,d, P);
                decrypted.Add(Convert.ToByte(x4));
            }

            const string prefix = "ShamirDec_";
            WriteBinaryDataToFile(decrypted.ToArray(), filepath, prefix);
      
        }
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
    }

    public class Rsa
    {
        public Rsa()
        {
            var rsaKeys = GenerateRsaKeys();
            _c = rsaKeys[0];
            D = rsaKeys[1];
            N = rsaKeys[2];
        }
        
        private readonly long _c;
        public long D { get; }
        public long N { get; }

        public long Encrypt(long m, long d, long n)
        {
            return CryptoLib.ModPow(m, d, n);
        }

        public long Decrypt(long m, long n)
        {
            return CryptoLib.ModPow(m, _c, n);
        }

        public void Encrypt(string filepath, long d, long n)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var encrypted = new List<long>();
            foreach (var item in buffer)
            {
                encrypted.Add(Encrypt(item, d, n));
            }

            const string prefix = "RsaEnc_";
            var encryptedFileName = prefix + Path.GetFileName(filepath);
            using var binWriter = new BinaryWriter(File.Open(encryptedFileName, FileMode.Create));
            foreach (long item in encrypted)
            {
                binWriter.Write(item);
            }

            binWriter.Close();
        }

        public void Decrypt(string filepath, long n)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var decrypted = new List<byte>();
            for (var i = 0; i < buffer.Length; i += 8)
            {
                var value = BitConverter.ToInt64(buffer, i);
                decrypted.Add(Convert.ToByte(CryptoLib.ModPow(value, _c, n)));
            }

            const string prefix = "RsaDec_";
            WriteBinaryDataToFile(decrypted.ToArray(), filepath, prefix);
        }

        private static List<long> GenerateRsaKeys()
        {
            long n, d, c;
            do
            {
                long p = CryptoLib.GenerateSimpleNumber(Int32.MaxValue);
                long q = CryptoLib.GenerateSimpleNumber(Int32.MaxValue);
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
            var buffer = ReadBinaryDataFromFile(filepath);
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= (byte)_code;
            }

            var prefix = "XorEnc_";
            WriteBinaryDataToFile(buffer, filepath, prefix);
        }

        public void Decrypt(string filepath)
        {
            Encrypt(filepath);
        }
    }

    private static byte[] ReadBinaryDataFromFile(string filepath)
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
        return buffer;
    }

    private static void WriteBinaryDataToFile(byte[] data, string filepath, string prefix)
    {
        var encryptedFileName = prefix + Path.GetFileName(filepath);
        using var binWriter = new BinaryWriter(File.Open(encryptedFileName, FileMode.Create));
        binWriter.Write(data);
        binWriter.Close();
    }
}