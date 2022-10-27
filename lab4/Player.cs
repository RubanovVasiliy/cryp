using crypto;

namespace lab4;

public class Player
{
    private long p;

    public Guid id { get; }

    public List<long> Cards { get; }

    public Player(long p)
    {
        id = Guid.NewGuid();
        Cards = new List<long>();
        this.p = p;
        
        List<long> gcd;
        do
        {
            c = new Random().NextInt64(p - 1);
            gcd = CryptoLib.Gcd(p - 1, c);
        } while (gcd[0] != 1 || gcd[2] < 0);

        d = gcd[2];
    }

    public long c { get; }
    public long d { get; }
}