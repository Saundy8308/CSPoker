using System;
using System.Collections.Generic;

namespace Poker
{
    class Program
    {

        static void Main(string[] args)
        {
            Game game = new Game(5);
            game.Play();
            Console.WriteLine("GAME OVER...");

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

    }

    class Game
    {
        private List<Player> players;
        private Pack pack;

        // Setting up game
        public Game(int numOfPlayers)
        {
            players.Add(new Player(new Hand(), true));

            for (int i = 0; i < numOfPlayers - 1; i++)
            {
                players.Add(new Computer(new Hand()));
            }

            pack = new Pack();
            pack.Shuffle();
        }

        // Called at the start of the game
        public void Play()
        {
            bool winner = false;
            // Keep going until winner is found
            while (!winner)
            {
                DoRound();
            }

        }

        public void DoRound()
        {
            Pack pack = new Pack();
            pack.Shuffle();
        }

        public void DisplayHandValue(List<Tuple<int, int>> list) // Display the output of PokerHand.GetValue() in a readable way
        {
            Console.Write("{");
            foreach (Tuple<int, int> tup in list)
            {
                Console.Write($"({tup.Item1},{tup.Item2}),");
            }
            Console.Write("}");
        }
    }

}
