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
        protected int chipsIn; // Number of chips that the player has currently betted
        protected string name; // Name of the player/computer

        protected bool currentPlayer; // Is this the player
        protected bool computer; // Is this a computer

        public Player(Hand hand, bool player, int startingChips = 500, string n = "Player")
        {
            pHand = hand;
            currentPlayer = player;
            chips = startingChips;
            chipsIn = 0;
            name = n;
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

        public int ChipsIn()
        {
            return chipsIn;
        }

        // Take bet
        public virtual int TakeBet(int matchBet) // Returns -1 if the player wants to Fold
        {
            Console.WriteLine($"How much would you like to bet (-1 to fold, {matchBet - chipsIn} to call/check): ");
            bool goodIn = false;
            int bet = 0;
            while (!goodIn)
            {
                string betIn = Console.ReadLine();
                goodIn = Int32.TryParse(betIn, out bet) && bet <= chips && bet + chipsIn >= matchBet;
            }

            chips -= bet;
            chipsIn += bet;
            return bet;
        }

        public string GetName()
        {
            return name;
        }

        public bool IsPlayer()
        {
            return currentPlayer;
        }

        public void Reset() // Used to reset a player at the end of a round
        {
            chipsIn = 0;
        }
    }

    class Computer : Player
    {
        public Computer(Hand hand) : base(hand, false, 500, "Computer")
        {
            computer = true;
        }

        // AI for valuing hand and placing bets
        public override int TakeBet(int matchBet)
        {
            Console.WriteLine(matchBet);

            List<Card> allCards = new List<Card>(pHand.GetCards());
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
            
            Random rnd = new Random();
            int rndInt = rnd.Next(10) - 5; // Random int from 5 to -5

            rndInt = 0; // TEST

            int val = roughValue + rndInt;
            if (val > 100) { val = 100; } // val is a number from 1 to 100 representing the hands value

            double percent = val / 100d;
            Console.WriteLine(percent); // TEST
            int bet = Convert.ToInt32(chips * percent);
            Console.WriteLine(bet); // TEST

            // RAISE, CHECK/CALL, FOLD
            rnd = new Random();
            rndInt = rnd.Next(100);

            if (rndInt < val && bet > matchBet)
            {
                // Raise
                Console.WriteLine($"{name} is Raising to {bet}...");
                return Bet(bet - chipsIn);
            }
            else if (rndInt > val * matchBet)
            {
                // Fold
                Console.WriteLine($"{name} is Folding...");
                return -1;
            }
            else
            {
                // Call/Check
                if (chipsIn == matchBet)
                {
                    Console.WriteLine($"{name} is Checking...");
                    return 0;
                }
                else
                {
                    Console.WriteLine($"{name} is Calling...");
                    return Bet(matchBet - chipsIn);
                }
            }

            Console.WriteLine("ERROR : TakeBet DIDN'T RETURN A VALUE, FOLDING");
            return -1;
        }

        // Easy method for betting an amount of chips
        public int Bet(int amount)
        {
            chipsIn += amount;
            chips -= amount;
            if (chips < 0) // ERROR CHECKING
            {
                Console.WriteLine("ERROR: chips is negative");
            }
            return amount;
        }
    }
}
