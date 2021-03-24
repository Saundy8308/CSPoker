using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    public class Card
    {
        private int rank;
        private int suit;

        public Card(int r, int s)
        {
            rank = r;
            suit = s;
        }

        public int GetRank()
        {
            return rank + 1; // Because of ace
        }

        public int GetSuit()
        {
            return suit;
        }

        public int GetScore()
        {
            int score = rank * 4 + suit;
            return score;
        }

        public string GetRankAsString()
        {
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            return ranks[rank - 1];
        }

        public string GetSuitAsString()
        {
            //♥♦♣♠
            string[] suits = { "♣", "♦", "♥", "♠" };
            return suits[suit - 1];
        }

        public override string ToString()
        {
            return GetRankAsString() + GetSuitAsString();
        }
    }
}
