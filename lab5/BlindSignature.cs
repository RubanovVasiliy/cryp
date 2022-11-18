using System.Numerics;
using System.Security.Cryptography;
using crypto;

namespace lab5;

public class BlindSignature
{
    public class Server
    {
        public BigInteger N { get; }
        private readonly BigInteger _c;
        public BigInteger D { get; }

        public readonly List<BigInteger> Voters = new();

        public int Yes;
        public int No;


        public Server()
        {
            BigInteger q;
            BigInteger p;
            do
            {
                p = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(1024);
            } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(p));

            do
            {
                q = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(1024);
            } while (!CryptoLib.PrimeExtensions.IsProbablyPrime(q) || p == q);

            N = p * q;
            var fi = (p - 1) * (q - 1);

            List<BigInteger> gcd;
            do
            {
                _c = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
                gcd = CryptoLib.Gcd(fi, _c);
                D = gcd[2];
            } while (gcd[0] != 1 || D < 0);
        }

        public BigInteger RegisterVoter(BigInteger h1)
        {
            Voters.Add(h1);
            return BigInteger.ModPow(h1, _c, N);
        }

        public bool CheckBulletin(BigInteger n, BigInteger s)
        {
            if (new BigInteger(SHA512.HashData(n.ToByteArray())) != BigInteger.ModPow(s, D, N))
            {
                return false;
            }

            if (Voters.Contains(s))
            {
                Console.WriteLine("Уже голосовал");
                return false;
            }

            var bytes = n.ToByteArray();

            var passport = BitConverter.ToInt32(bytes, 64);
            var vote = bytes[^1];
            Console.WriteLine("passport: {0} vote: {1}", passport, vote);

            switch (vote)
            {
                case 1:
                    No++;
                    break;
                case 2:
                    Yes++;
                    break;
                default:
                    Console.WriteLine("Error, incorrect vote");
                    return false;
            }

            Voters.Add(s);
            return true;
        }
    }


    public class Voter
    {
        public BigInteger H1 { get; }
        private readonly BigInteger _n;
        private readonly BigInteger r;
        public BigInteger S { get; set; }
        public BigInteger Message { get; set; }


        public Voter(BigInteger N, BigInteger D, int passport, byte vote)
        {
            _n = N;
            var rnd = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(512);
            var passBitArr = new BigInteger(passport).ToByteArray();
            if (passBitArr.Length != 4)
            {
                passBitArr = passBitArr.Concat(new byte[] { 0 }).ToArray();
            }

            var voteBitArr = new BigInteger(vote).ToByteArray();
            var temp3 = rnd.ToByteArray()
                .Concat(passBitArr)
                .Concat(voteBitArr)
                .ToArray();
            Message = new BigInteger(temp3);

            List<BigInteger> gcd;
            do
            {
                r = CryptoLib.PrimeExtensions.RandomIntegerSizeBit(256);
                gcd = CryptoLib.Gcd(N, r);
            } while (gcd[0] != 1);

            var h = SHA512.HashData(Message.ToByteArray());
            H1 = new BigInteger(h) * BigInteger.ModPow(r, D, N) % N;
        }

        public void CreateS(BigInteger s)
        {
            var rInv = ModInverse(r, _n);
            S = s * rInv % _n;
        }
    }

    public static BigInteger ModInverse(BigInteger n, BigInteger p)
    {
        var inv = CryptoLib.Gcd(p, n)[2];
        if (inv < 0)
        {
            inv += p;
        }

        return inv;
    }
}