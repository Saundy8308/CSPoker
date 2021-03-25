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
            Player[] playersArr = new Player[numOfPlayers];

            playersArr[0] = (new Player(new PokerHand(), true));

            for (int i = 1; i < numOfPlayers; i++)
            {
                playersArr[i] = (new Computer(new PokerHand()));
            }

            players = new List<Player>(playersArr);
            
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
                CleanPlayers();
                if (players.Count <= 1)
                {
                    winner = true;
                }
            }

        }

        // Do a round
        public void DoRound()
        {
            pack.Shuffle();

            Hand communityCards = new Hand();
            Draw(communityCards, 5);

            for (int i = 0; i < players.Count; i++)
            {
                Draw(players[i].pHand, 2);
            }

            communityCards.DisplayHand();

            foreach (Player p in players)
            {
                p.pHand.DisplayHand();
            }
        }

        // Used to easily transfer crds from pack to a hand
        public void Draw(Hand h, int amnt)
        {
            for (int i = 0; i < amnt; i++)
            {
                h.AddCard(pack.DealCard());
            }
        }

        // Used to check if there are any players with 0 chips if so, removes them
        public void CleanPlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].HasChips())
                {
                    players.RemoveAt(i);
                    i--;                   //##################### POSSIBLE ERROR HERE
                }
            }
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
