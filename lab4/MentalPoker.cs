using crypto;

namespace lab4;

public static class MentalPoker
{
    private static class RandomExtensions
    {
        public static void Shuffle<T>(Random rng, IList<T> array)
        {
            var n = array.Count;
            while (n > 1)
            {
                var k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }

    public static void TexasHoldem(List<long> cards, int playersCount = 5)
    {
        const int cardsOnTAbleCount = 5;
        if (cards.Count < playersCount * 2 + cardsOnTAbleCount)
        {
            throw new Exception("too few cards");
        }

        var p = CryptoLib.GenerateSimpleNumber(1000000000, true);

        var players = new List<Player>();
        for (var i = 0; i < playersCount; i++)
        {
            players.Add(new Player(p));
        }

        // encrypt all cards 
        foreach (var player in players)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                cards[i] = CryptoLib.ModPow(cards[i], player.c, p);
            }

            RandomExtensions.Shuffle(new Random(), cards);
        }

        // deal cards to players
        foreach (var player in players)
        {
            for (var i = 0; i < 2; i++)
            {
                player.Cards.Add(cards[0]);
                cards.Remove(cards[0]);
            }
        }

        // print encrypted cards 
        // foreach (var player in players)
        // {
        //     Console.WriteLine(player.id);
        //     foreach (var card in player.Cards)
        //     {
        //         Console.WriteLine(card);
        //     }
        //     Console.WriteLine();
        // }

        // decrypt users cards
        foreach (var player in players)
        {
            for (var i = 0; i < player.Cards.Count; i++)
            {
                foreach (var ps in players)
                {
                    player.Cards[i] = CryptoLib.ModPow(player.Cards[i], ps.d, p);
                }
            }
        }

        var cardsOnTable = new List<long> { Capacity = cardsOnTAbleCount };
        for (var i = 0; i < cardsOnTAbleCount; i++)
        {
            cardsOnTable.Add(cards[0]);
            cards.Remove(cards[0]);
        }

        // decrypt cards on table 
        for (var i = 0; i < cardsOnTable.Count; i++)
        {
            foreach (var player in players)
            {
                cardsOnTable[i] = CryptoLib.ModPow(cardsOnTable[i], player.d, p);
            }
        }
        
        // print decrypted cards 
        foreach (var player in players)
        {
            Console.WriteLine(player.id);
            foreach (var card in player.Cards)
            {
                Console.WriteLine(card);
            }

            Console.WriteLine();
        }
        
        // print decrypted cards 
        foreach (var card in cardsOnTable)
        {
            Console.WriteLine(card);
        }
    }
}