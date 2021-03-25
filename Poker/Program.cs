using System;
using System.Collections.Generic;

namespace Poker
{
    class Program
    {

        static void Main(string[] args)
        {
            Play();

            //PokerHand p1hand = new PokerHand();

            /*p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard());
            p1hand.AddCard(pack.DealCard()); *///temp for testing

            /*p1hand.AddCard(new Card(8,4));
            p1hand.AddCard(new Card(8,1));
            p1hand.AddCard(new Card(8,2));
            p1hand.AddCard(new Card(2,3));
            p1hand.AddCard(new Card(2,2));*/
        }

        public static void Play()
        {
            Pack pack = new Pack();
            pack.Shuffle();


        }

        public static void DoRound()
        {
            
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
