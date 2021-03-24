using System;
using System.Collections.Generic;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            Pack pack = new Pack();
            pack.Shuffle();
            PokerHand p1hand = new PokerHand();

            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard()); //temp for testing

            /*p1hand.AddCard(new Card(8,4));
            p1hand.AddCard(new Card(8,1));
            p1hand.AddCard(new Card(8,2));
            p1hand.AddCard(new Card(2,3));
            p1hand.AddCard(new Card(2,2));*/

            p1hand.Order();
            p1hand.DisplayHand();

            List<Tuple<int, int>> temp = p1hand.GetValue();
            Console.WriteLine(temp.Count);
            DisplayHandValue(temp);
        }

        public static void DisplayHandValue(List<Tuple<int,int>> list) // Display the output of PokerHand.GetValue() in a readable way
        {
            Console.Write("{");
            foreach (Tuple<int,int> tup in list)
            {
                Console.Write($"({tup.Item1},{tup.Item2}),");
            }
            Console.Write("}");
        }
    }
}
