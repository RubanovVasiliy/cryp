using System.Numerics;
using System.Security.Cryptography;

namespace crypto;

public class Encryption
{
    private const int MaxValue = 1000000000;

    public class ElGamal
    {
        public ElGamal()
        {
            var n = 1000000000;
            P = CryptoLib.GenerateSimpleNumber(n, true);
            Q = (P - 1) / 2;
            do
            {
                G = CryptoLib.GenerateSimpleNumber(P - 1);
            } while (CryptoLib.ModPow(G, Q, P) != 1);

            CA = CryptoLib.GenerateSimpleNumber(P - 1);
            DA = CryptoLib.ModPow(G, CA, P);

            CB = CryptoLib.GenerateSimpleNumber(P - 1);
            DB = CryptoLib.ModPow(G, CB, P);
        }

        public int P { get; }
        public int Q { get; }
        public int G { get; }
        public int CA { get; }
        public long DA { get; }
        public int CB { get; }
        public long DB { get; }

        private const string KeysFilePath = "keys.bin";


        public void Encrypt(string filepath)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var encrypted = new List<long>();
            var keys = new List<long>();
            foreach (var item in buffer)
            {
                var k = CryptoLib.GenerateSimpleNumber(P - 1);
                var r = CryptoLib.ModPow(G, k, P);
                var e = item * CryptoLib.ModPow(DB, k, P) % P;
                encrypted.Add(e);
                keys.Add(r);
            }

            const string prefix = "ElGamalEnc_";
            WriteLongToBinDataToFile(encrypted, filepath, prefix);
            WriteLongToBinDataToFile(keys, KeysFilePath);
        }

        public void Decrypt(string filepath)
        {
            var encryptedData = ReadBinaryDataFromFile(filepath);
            var keysData = ReadBinaryDataFromFile(KeysFilePath);

            var decrypted = new List<byte>();
            for (var i = 0; i < encryptedData.Length; i += 8)
            {
                var value = BitConverter.ToInt64(encryptedData, i);
                var key = BitConverter.ToInt64(keysData, i);
                var decryptedMessage = value * CryptoLib.ModPow(key, P - 1 - CB, P) % P;
                decrypted.Add(Convert.ToByte(decryptedMessage));
            }

            const string prefix = "ElGamalDec_";
            WriteBinaryDataToFile(decrypted.ToArray(), filepath, prefix);
        }

        public long EncryptDigitalSignature(string filepath)
        {
            var buffer = ReadBinaryDataFromFile(filepath);
            var sha512 = SHA256.Create().ComputeHash(buffer);

            var x = new Random().NextInt64(P - 1);
            var y = CryptoLib.ModPow(G, x, P);

            long k;
            List<long> gcd;
            do
            {
                k = new Random().NextInt64(P - 1);
                gcd = CryptoLib.Gcd(P - 1, k);
            } while (gcd[0] != 1 || gcd[2] < 0);

            var r = CryptoLib.ModPow(G, k, P);
            var u = (new BigInteger(sha512) - x * r) % (P - 1);
            var s = (long)(gcd[2] * u % (P - 1));

            var digitalSignature = new List<long> { r, s };
            const string prefix = "ElGamalDigitalSignature_";
            WriteLongToBinDataToFile(digitalSignature, filepath, prefix);
            return y;
        }

        public static bool CheckDigitalSignature(string filepath, long y, long p, long g)
        {
            var buffer = ReadBinaryDataFromFile(filepath);
            var sha512 = SHA256.Create().ComputeHash(buffer);

            const string prefix = "ElGamalDigitalSignature_";
            var dSBuffer = ReadBinaryDataFromFile(prefix + filepath);

            var r = BitConverter.ToInt32(dSBuffer, 0);
            var s = BitConverter.ToInt32(dSBuffer, 8);

            var res = BigInteger.ModPow(y, r, p) * BigInteger.ModPow(r, s, p) % p;
            var rs = BigInteger.ModPow(g, new BigInteger(sha512), p);

            return res == rs;
        }
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
            return CryptoLib.ModPow(m, D, P);
        }

        public void Encrypt(string filepath, long c)
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

        public void Decrypt(string filepath, long d)
        {
            var buffer = ReadBinaryDataFromFile(filepath);

            var decrypted = new List<byte>();
            for (var i = 0; i < buffer.Length; i += 8)
            {
                var value = BitConverter.ToInt64(buffer, i);
                var x3 = CryptoLib.ModPow(value, D, P);
                var x4 = CryptoLib.ModPow(x3, d, P);
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
                long p = CryptoLib.GenerateSimpleNumber(int.MaxValue);
                long q = CryptoLib.GenerateSimpleNumber(int.MaxValue);
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

        public void EncryptDigitalSignature(string filepath)
        {
            var buffer = ReadBinaryDataFromFile(filepath);
            var sha512 = SHA512.Create().ComputeHash(buffer);

            var digitalSignature = new List<long>();
            for (var i = 0; i < sha512.Length; i += 4)
            {
                var value = BitConverter.ToInt32(sha512, i);
                var s = CryptoLib.ModPow(value, _c, N);
                digitalSignature.Add(s);
            }

            const string prefix = "RsaDigitalSignature_";
            WriteLongToBinDataToFile(digitalSignature, filepath, prefix);
        }

        public static bool CheckDigitalSignature(string filepath, long d, long n)
        {
            const string prefix = "RsaDigitalSignature_";
            var dSBuffer = ReadBinaryDataFromFile(prefix + filepath);
            var digitalSignature = new List<long>();
            for (var i = 0; i < dSBuffer.Length; i += 8)
            {
                var value = BitConverter.ToInt64(dSBuffer, i);
                var s = CryptoLib.ModPow(value, d, n);
                digitalSignature.Add(s);
            }

            var buffer = ReadBinaryDataFromFile(filepath);
            var sha512 = SHA512.Create().ComputeHash(buffer);
            var hash = new List<long>();
            for (var i = 0; i < sha512.Length; i += 4)
            {
                var value = BitConverter.ToInt32(sha512, i);
                hash.Add(value);
            }

            return digitalSignature.SequenceEqual(hash);
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

    private static void WriteLongToBinDataToFile(List<long> data, string filepath, string prefix = "")
    {
        var encryptedFileName = prefix + Path.GetFileName(filepath);
        using var binWriter = new BinaryWriter(File.Open(encryptedFileName, FileMode.Create));
        foreach (var item in data)
        {
            binWriter.Write(item);
        }

        binWriter.Close();
    }
}