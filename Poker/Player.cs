using System;
using System.Collections.Generic;

namespace Poker
{
    // Player class used to define anyone playing the game AI or otherwise
    class Player
    {
        public PokerHand pHand;

        protected int chips;
        protected string name; // Implement later on

        protected bool currentPlayer; // Is this the player
        protected bool computer; // Is this a computer

        public Player(PokerHand hand, bool player, int chips = 500)
        {
            pHand = hand;
            currentPlayer = player;
        }

        // Returns the cards that were in the hand so they can be put back in the deck
        public PokerHand ClearHand()
        {
            PokerHand temp = pHand;
            pHand.Clear();
            return temp;
        }

        // Returns wether or not the amount can be removed
        public bool BetChips(int amount)
        {
            if (chips - amount > 0)
            {
                chips -= amount;
                return true;
            }
            else
            {
                return false;
            }
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
    }

    class Computer : Player
    {
        public Computer(PokerHand hand) : base(hand, false)
        {
            computer = true;
        }
    }
}
