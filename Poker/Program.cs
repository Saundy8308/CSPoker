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

            playersArr[0] = (new Player(new Hand(), true));

            for (int i = 1; i < numOfPlayers; i++)
            {
                playersArr[i] = (new Computer(new Hand()));
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
                Console.WriteLine("Round over");
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

            List<Player> playersIn = players; // PlayersIn are the players that haven't folded this round

            int communityCardsShown = 0; // Represents the number of community cards visible to the player
            int pot = 0; // Number of chips in the pot

            // Dealing cards to players
            for (int i = 0; i < playersIn.Count; i++)
            {
                Draw(playersIn[i].pHand, 2);
            }

            bool roundOver = false;
            bool playerFolded = false;
            while (!roundOver)
            {
                // Displaying information about the round if the player is still in
                if (!playerFolded)
                {
                    Console.WriteLine($"POT:        {pot}");
                    Console.WriteLine($"YOUR CHIPS: {players[0].Chips()}");
                    Console.Write("\nYOUR HAND:  ");
                    players[0].pHand.DisplayHand();
                    Console.Write("COMMUNITY:  ");
                    communityCards.DisplayHand(true, communityCardsShown);
                    Console.WriteLine();
                }

                // Taking bets
                for (int i = 0; i < playersIn.Count; i++)
                {
                    int pBet = playersIn[i].TakeBet(); // Returns -1 if they want to fold
                    if (pBet >= 0) // not folding
                    {
                        pot += pBet;
                    }
                    else // folding
                    {
                        if (playersIn[i].IsPlayer())
                        {
                            playerFolded = true;
                        }

                        playersIn.RemoveAt(i);
                        i--;
                    }
                }

                if (playersIn.Count <= 1)
                {
                    roundOver = true;
                }

                communityCardsShown++;
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
