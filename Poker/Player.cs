using System;
using System.Collections.Generic;

namespace Poker
{
    // Player class used to define anyone playing the game AI or otherwise
    class Player
    {
        public Hand pHand;
        protected Hand communityCards;

        protected int chips;
        protected string name; // Implement later on

        protected bool currentPlayer; // Is this the player
        protected bool computer; // Is this a computer

        public Player(Hand hand, bool player, int startingChips = 500)
        {
            pHand = hand;
            currentPlayer = player;
            chips = startingChips;
        }

        public void SetCommCards(Hand commCards)
        {
            communityCards = commCards;
        }

        // Returns the cards that were in the hand so they can be put back in the deck
        public Hand ClearHand()
        {
            Hand temp = pHand;
            pHand.Clear();
            return temp;
        }

        // Adds chips to 'chips'
        public void AddChips(int amount)
        {
            chips += amount;
        }
        // As expected returns wether or nor this player has chips
        public bool HasChips()
        {
            return chips > 0;
        }
        public int Chips()
        {
            return chips;
        }

        // Take bet
        public virtual int TakeBet() // Returns -1 if the player wants to Fold
        {
            Console.WriteLine("How much would you like to bet (-1 to fold): ");
            bool goodIn = false;
            int bet = 0;
            while (!goodIn)
            {
                string betIn = Console.ReadLine();
                goodIn = Int32.TryParse(betIn, out bet) && bet<=chips;
            }

            chips -= bet;
            return bet;
        }

        public bool IsPlayer()
        {
            return currentPlayer;
        }
    }

    class Computer : Player
    {
        public Computer(Hand hand) : base(hand, false)
        {
            computer = true;
        }

        // AI for valuing hand and placing bets
        public override int TakeBet()
        {
            List<Card> allCards = pHand.GetCards();
            allCards.AddRange(communityCards.GetCards());

            PokerHand totalHand = new PokerHand();
            totalHand.SetCards(allCards);

            List<Tuple<int,int>> value = totalHand.GetValue();

            // Most of this is done randomly just to generate an amount to bet

            int roughValue = 0;

            foreach(Tuple<int,int> tup in value)
            {
                roughValue += tup.Item1 * tup.Item2 / 4;
            }
            if (roughValue > 50)
            {
                roughValue = 50;
            }
            Random rnd = new Random();
            int rndInt = rnd.Next(10) - 5; // Random int from 5 to -5

            rndInt = 0; // For testing

            double percent = (roughValue + rndInt) / 100d;
            Console.WriteLine(percent);
            int bet = Convert.ToInt32(chips * percent);

            Console.WriteLine(bet); // TEST
            return bet;
        }
    }
}
