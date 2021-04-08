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
        protected string name; // Implement later on

        protected bool currentPlayer; // Is this the player
        protected bool computer; // Is this a computer

        public Player(Hand hand, bool player, int startingChips = 500)
        {
            pHand = hand;
            currentPlayer = player;
            chips = startingChips;
            chipsIn = 0;
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
        public Computer(Hand hand) : base(hand, false)
        {
            computer = true;
        }

        // AI for valuing hand and placing bets
        public override int TakeBet(int matchBet)
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
            
            Random rnd = new Random();
            int rndInt = rnd.Next(10) - 5; // Random int from 5 to -5

            rndInt = 0; // For testing TEST

            double percent = (roughValue + rndInt) / 100d;
            Console.WriteLine(percent); // TEST
            int bet = Convert.ToInt32(chips * percent);

            Console.WriteLine(bet); // TEST

            /*
             I THINK THERE IS A MUCH SMOOTHER WAY OF DOING THIS
             \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
             */


            if (chipsIn < matchBet) // The AI must bet higher to stay in (Call, Raise or Fold)
            {
                if (bet < matchBet) // The AI predicts their hand is worth less than the current bet
                {
                    rnd = new Random();
                    bool match = rnd.Next(2) == 0; // Whether or not the AI will match the current bet (very basic)

                    if (match)
                    {
                        Console.WriteLine("Call");
                        chips -= matchBet - bet;
                        chipsIn += matchBet - bet;
                        return matchBet - bet;
                    }
                    else
                    {
                        Console.WriteLine("Fold");
                        return -1;
                    }
                }
                else if (bet >= matchBet) // The AI predicts their hand is worth more than or equal to the current bet
                {
                    rnd = new Random();
                    bool match = rnd.Next(2) == 0; // Whether or not the AI will match the current bet (very basic)

                    if (match)
                    {
                        Console.WriteLine("Call");
                        chips -= matchBet - bet;
                        chipsIn += matchBet - bet;
                        return matchBet - bet;
                    }
                    else
                    {
                        Console.WriteLine("Raise");
                        chips -= bet - matchBet;
                        chipsIn += bet - matchBet;
                        return bet - matchBet;
                    }
                }
            }

            else if (chipsIn > matchBet) // This SHOULD never happen because matchBet should always be the highest number of chipsIn
            {
                Console.WriteLine("ERROR : chipsIn is higher thatn matchBet");
                return -1;
            }

            else // chipsIn and matchBet are the same, the AI can check if they want (Check, Fold or Raise)
            {
                if (bet < matchBet) // The AI predicts their hand is worth less than the current bet
                {
                    rnd = new Random();
                    bool match = rnd.Next(2) == 0; // Whether or not the AI will match the current bet (very basic)

                    if (match)
                    {
                        Console.WriteLine("Check");
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("Fold");
                        return -1;
                    }
                }
                else if (bet >= matchBet) // The AI predicts their hand is worth more than or equal to the current bet
                {
                    rnd = new Random();
                    bool match = rnd.Next(2) == 0; // Whether or not the AI will match the current bet (very basic)

                    if (match)
                    {
                        Console.WriteLine("Check");
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("Raise");
                        chips -= bet - matchBet;
                        chipsIn += bet - matchBet;
                        return bet - matchBet;
                    }
                }
            }

            Console.WriteLine("ERROR : TakeBet DIDN'T RETURN A VALUE, FOLDING");
            return -1;
        }
    }
}
