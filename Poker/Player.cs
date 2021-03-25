using System;
using System.Collections.Generic;

namespace Poker
{
    // Player class used to define anyone playing the game AI or otherwise
    class Player
    {
        protected Hand pHand;
        protected int chips;

        protected bool currentPlayer; // Is this the player
        protected bool computer; // Is this a computer

        public Player(Hand hand, bool player, int chips = 500)
        {
            pHand = hand;
            currentPlayer = player;
        }

        // Returns the cards that were in the hand so they can be put back in the deck
        public Hand ClearHand()
        {
            Hand temp = pHand;
            pHand.Clear();
            return temp;
        }

        // Returns number of chips for easy use
        public int BetChips(int amount)
        {
            chips -= amount;
            return amount;
        }

        // Adds chips to 'chips'
        public void AddChips(int amount)
        {
            chips += amount;
        }
    }

    class Computer : Player
    {
        public Computer(Hand hand) : base(hand, false)
        {
            computer = true;
        }
    }
}
