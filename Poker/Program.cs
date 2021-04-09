﻿using System;
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

        private int STARTINGCHIPS;

        // Setting up game
        public Game(int numOfPlayers)
        {
            STARTINGCHIPS = 500;

            Player[] playersArr = new Player[numOfPlayers];
            Console.WriteLine("ENTER NAME: ");
            string name = Console.ReadLine();

            playersArr[0] = (new Player(new Hand(), true, STARTINGCHIPS, name));

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
            

            List<Player> playersIn = new List<Player>(players); // PlayersIn are the players that haven't folded this round

            int pot = 0; // Number of chips in the pot
            int bet = 0; // The amount that a player must match to remain in the game

            // Dealing cards to players
            for (int i = 0; i < playersIn.Count; i++)
            {
                Draw(playersIn[i].pHand, 2);
            }

            bool roundOver = false;
            bool playerFolded = false;
            int roundNum = 0;
            int[] toDeal = { 0, 2, 1, 1, 1 }; // The number of cards to deal to commCards each round
            while (!roundOver)
            {
                // Drawing cards to the community hand
                try
                {
                    Draw(communityCards, toDeal[roundNum]);
                }
                catch
                {
                    Console.WriteLine("All cards drawn...");
                    roundOver = true;
                    break;
                }

                roundNum++;
                // Displaying information about the round if the player is still in
                if (!playerFolded)
                {
                    Console.WriteLine($"\nPOT:        {pot}");
                    Console.WriteLine($"YOUR CHIPS: {players[0].Chips()}");
                    Console.Write("\nYOUR HAND:  ");
                    players[0].pHand.DisplayHand();
                    Console.Write("COMMUNITY:  ");
                    communityCards.DisplayHand(true, 5);
                    Console.WriteLine();
                }

                // Taking bets
                for (int i = 0; i < playersIn.Count; i++)
                {
                    Console.WriteLine($"{playersIn[i].GetName()}'s turn");
                    
                    playersIn[i].SetCommCards(communityCards);

                    playersIn[i].pHand.DisplayHand();
                    int pBet = playersIn[i].TakeBet(bet); // Returns -1 if they want to fold

                    if (pBet >= 0) // not folding
                    {
                        pot += pBet;
                        bet = playersIn[i].ChipsIn();
                        Console.WriteLine(bet + " =bet"); // TEST
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
            }

            // Calculating winner


            // ROUND IS OVER, CLEARING COMMUNITY CARDS
            pack.AddCard(communityCards);
        }

        // Used to easily transfer cards from pack to a hand
        public void Draw(Hand h, int amnt)
        {
            for (int i = 0; i < amnt; i++)
            {
                h.AddCard(pack.DealCard());
            }
        }

        // Used to check if there are any players with 0 chips if so, removes them also clears each players hand and adds it back to pack
        public void CleanPlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                pack.AddCard(players[i].ClearHand());
                players[i].Reset();

                if (!players[i].HasChips())
                {
                    players.RemoveAt(i);
                    i--;                   //##################### POSSIBLE ERROR HERE
                }
            }
        }

    }

}
