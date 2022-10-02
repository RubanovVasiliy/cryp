using System;

namespace crypto
{
    public class CryptoLib
    {
        public static Int64 Mod(Int64 a, Int64 x, Int64 p)
        {
            if (x == 0) return 1;
            if (p <= 0) throw new Exception("P can not be <= 0");
            if (x < 0) throw new Exception("X can not be < 0");

            var t = Convert.ToInt16(Math.Log(x) / Math.Log(2));
            var num = x;
            Int64 result = 1;
            var temp = a;

            for (var i = 0; i <= t; i++)
            {
                if (Convert.ToBoolean(num & 1))
                    result = (result * temp) % p;
                temp = (temp * temp) % p;
                num = num >> 1;
            }

            return result;
        }

        public static List<Int64> Gcd(Int64 a, Int64 b)
        {
            if (a < b) throw new Exception("In GCD must be a >= b");

            var u = new List<Int64> { a, 1, 0 };
            var v = new List<Int64> { b, 0, 1 };
            var t = new List<Int64> { 0, 0, 0 };

            while (v[0] != 0)
            {
                var q = u[0] / v[0];
                t[0] = u[0] % v[0];
                t[1] = u[1] - q * v[1];
                t[2] = u[2] - q * v[2];

                u = new List<long>(v);
                v = new List<long>(t);
            }

            return u;
        }

        public static List<long> Shanks(long a, long p, long y)
        {
            if (p == 0) throw new Exception("P can not be 0");
            var k = Convert.ToInt64(Math.Sqrt(p));
            var map = new Dictionary<long, List<long>>();
            var keysX = new List<long>();

            for (var i = 0; i < k; i++)
            {
                var temp = (Mod(a, i, p) * y % p) % p;
                if (map.ContainsKey(temp))
                {
                    map.TryGetValue(temp, out var item);
                    item!.Add(i);
                    map.Remove(temp);
                    map.TryAdd(temp, item);
                }
                else
                {
                    map.TryAdd(temp, new List<long>() { i });
                }
            }

            for (var i = 1; i <= k; i++)
            {
                var temp = Mod(a, k * i, p);

                if (map.TryGetValue(temp, out var item))
                {
                    foreach (var it in item)
                    {
                        var result = i * k - it;
                        keysX.Add(result);
                    }
                }
            }

            return keysX;
        }

        public static bool IsSimple(Int64 n)
        {
            if (n <= 1) return false;

            var b = Convert.ToInt64(Math.Pow(n, 0.5));

            for (int i = 2; i <= b; ++i)
            {
                if ((n % i) == 0) return false;
            }

            return true;
        }

        public static List<Int64> EratosthenesSieve(Int64 n)
        {
            var list = new List<long>();
            for (var t = 0; t <= n; t++)
                list.Add(t);

            list[1] = 0;
            var i = 2;
            while (i <= n)
            {
                if (list[i] != 0)
                {
                    var j = i + i;
                    while (j <= n)
                    {
                        list[j] = 0;
                        j = j + i;
                    }
                }

                i += 1;
            }

            var set = new HashSet<long>(list);
            set.Remove(0);
            return set.ToList();
        }

        public static int GenerateSimpleNumber(int n, bool strong = false)
        {
            int rnd;
            do
            {
                rnd = new Random().Next(n);
            } while (!IsSimple(rnd) || !(!strong || IsSimple((rnd - 1) / 2)));

            return rnd;
        }

        public static bool DiffieHellman()
        {
            const int n = 1000000000;
            var p = GenerateSimpleNumber(n, true);
            var q = (p - 1) / 2;
            long g;
            do
            {
                g = GenerateSimpleNumber(n);
            } while (p - 1 > g && Mod(g, q, p) != 1);

            var Xa = GenerateSimpleNumber(n);
            var Xb = GenerateSimpleNumber(n);

            var Ya = Mod(g, Xa, p);
            var Yb = Mod(g, Xb, p);

            var Zab = Mod(Yb, Xa, p);
            var Zba = Mod(Ya, Xb, p);

            Console.WriteLine("P = {0}\nQ = {1}\nG = {2}", p, q, g);
            Console.WriteLine("Xa = {0}\nXb = {1}", Xa, Xb);
            Console.WriteLine("Ya = {0}\nYb = {1}", Ya, Yb);
            Console.WriteLine("Zab = {0}\nZab = {1}", Zab, Zba);
            return Zab == Zba;
        }

        private static List<long> GenerateShamirKeys(int p)
        {
            var rnd = new Random();
            int c;
            List<long> gcd;
            do
            {
                c = rnd.Next(p - 1);
                gcd = Gcd(p - 1, c);
            } while (!IsSimple(c) && gcd[0] != 1);

            var d = gcd[2];
            if (d < 0)
            {
                d += p - 1;
            }

            return new List<long>() { c, d };
        }

        public static bool Shamir(long m)
        {
            var n = 1000000000;
            var p = GenerateSimpleNumber(n);
            var aShamirKeys = GenerateShamirKeys(p);
            var bShamirKeys = GenerateShamirKeys(p);


            var x1 = Mod(m, aShamirKeys[0], p);
            var x2 = Mod(x1, bShamirKeys[0], p);
            var x3 = Mod(x2, aShamirKeys[1], p);
            var x4 = Mod(x3, bShamirKeys[1], p);

            Console.WriteLine("{0} {1}", m, x4);
            
            return m == x4;
        }
    }
}