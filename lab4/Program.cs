// See https://aka.ms/new-console-template for more information

using crypto;
using lab4;

Console.WriteLine("Hello, World!");

var cards = new List<long>
{
 Capacity = 52
};
 
for (var i = 2; i < 53; i++)
{
 cards.Add(i);
}


MentalPoker.TexasHoldem(cards);