using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    public class Hand
    {
        // Hand is a collection of previously-created cards, typically contained
        // in Pack.Cards
        // Hand does not create any Cards itself

        protected List<Card> cards = new List<Card>();

        public Card this[int i]
        {
            get { return cards[i]; }
            // this provides read-only access to the List by index
        }

        public int Size
        {
            get
            {
                return cards.Count;
            }
        }
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int FindCard(int r, int s)
        // find the position of a specified card in the hand
        // returns -1 if not found
        // useful in rummy-type games
        {
            int result = -1;
            for (int i = 0; i <= Size; i++)
            {
                if ((cards[i].GetRank() == r) && (cards[i].GetSuit() == s))
                {
                    result = i;
                }
            }
            return result;
            //returns -1 if not present
        }

        public void DisplayHand()
        {
            foreach(Card card in cards)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public Card First()
        {
            return cards[0];
        }

        public bool IsEmpty()
        {
            return Size == 0;
        }

        public Card Last()
        {
            return cards[Size - 1];
        }

        public Card RemoveCard(int i)
        // remove card from the hand, by index
        {
            if (Size > i)
            {
                Card result = cards[i];
                cards.RemoveAt(i);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Card RemoveFirstCard()
        {
            if (!IsEmpty())
            {
                Card c = cards[0];
                cards.RemoveAt(0);
                return c;
            }
            else
            {
                return null;
            }

        }

        public void Clear()
        {
            cards.Clear();
        }

    }

    // This is the actual hand that a player will have
    public class PokerHand : Hand
    {
        // Returns the 'value' of the hand based on the scoring in hands.txt
        public List<Tuple<int,int>> GetValue()
        {
            Order();

            List<Tuple<int,int>> sets = new List<Tuple<int,int>>(); // A list of the value of the hands and their value when compared to the same hand. Tuple(Hand,HandValue)

            bool flush = true;   // Used to check if there is a flush in the hand
            bool straight = true; // Used to check if there is a straight in the hand

            int flushSuit = cards[0].GetSuit(); // 1:♣  2:♦  3:♥  4:♠

            int highest = cards[0].GetRank();

            for (int i = 0; i < cards.Count; i++)
            {
                // Checks if a flush is possible
                if (flush)
                {
                    if (cards[i].GetSuit() != flushSuit)
                    {
                        flush = false; // A flush isn't possible
                    }
                }

                if (i != cards.Count - 1) //Straight can only be checked on the card before last
                {
                    // Checks if a straight is possible
                    if (straight)
                    {
                        if (cards[i + 1].GetRank() != cards[i].GetRank() - 1)
                        {
                            straight = false; // A straight isn't possible
                        }
                    }
                }

                bool added = false;
                if(sets.Count == 0) // If sets is empty, don't iterate through, add a new tuple
                {
                    sets.Add(new Tuple<int, int>(1, cards[i].GetRank()));
                    added = true;
                }
                else for (int ii = 0; ii < sets.Count; ii++) // Otherwise iterate through to see if there are multiple of the current card
                {
                        if (sets[ii].Item2 == cards[i].GetRank())
                        {
                            int temp = sets[ii].Item1;
                            Console.WriteLine(temp);
                            sets[ii] = new Tuple<int, int>(temp + 1, cards[i].GetRank());
                            added = true;
                        }
                }
                if (!added) // If there isn't multiple just add new tuple
                {
                    sets.Add(new Tuple<int, int>(1, cards[i].GetRank()));
                }
            }

            // Iterate through again checking for multiple cards
            // Because four of a kind is worth more than 4 (8)
            // Because three of a kind is worth more than 3 (4)
            int pairs = 0; // Number of pairs in hand
            int tok = 0; // Value of Three of a kind in hand 0 if no TOK
            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i].Item1 == 2)
                {
                    pairs++;
                }
                else if (sets[i].Item1 == 3)
                {
                    int temp = sets[i].Item2;
                    sets[i] = new Tuple<int, int>(4, temp);
                    tok = temp;
                }
                else if (sets[i].Item1 == 4)
                {
                    int temp = sets[i].Item2;
                    sets[i] = new Tuple<int, int>(8, temp);
                }
            }

            if (flush && straight) // Straight flush or royal flush
            {
                if(highest == 14) // Royal flush
                {
                    sets = new List<Tuple<int, int>>() { new Tuple<int, int>(10, 14) };
                }
                else // Straight flush
                {
                    sets = new List<Tuple<int, int>>() { new Tuple<int, int>(9, highest) };
                }
            }
            else if (pairs==1 && tok>0) // check for a Full house (a pair and a three of a kind)
            {
                sets = new List<Tuple<int, int>>() { new Tuple<int, int>(7, tok) };
            }
            else if (straight)
            {
                sets = new List<Tuple<int, int>>() { new Tuple<int, int>(5, highest) };
            }
            else if (flush)
            {
                sets = new List<Tuple<int, int>>(){ new Tuple<int,int>(6, highest) };
            }

            // Order sets:
            bool done = false;
            // While the hand is ordered
            while (!done)
            {
                done = true;
                // Loop through and swap cards positions
                for (int i = 0; i < sets.Count - 1; i++)
                {
                    if (sets[i].Item1 < sets[i + 1].Item1)
                    {
                        Tuple<int,int> temp = sets[i + 1];
                        sets[i + 1] = sets[i];
                        sets[i] = temp;
                        done = false;
                    }
                }
            }

            return sets;
        }

        // Cards are ordered largest to smallest
        public void Order()
        {
            bool done = false;
            // While the hand is ordered
            while (!done)
            {
                done = true;
                // Loop through and swap cards positions
                for (int i = 0; i < cards.Count - 1; i++)
                {
                    if (cards[i].GetRank() < cards[i + 1].GetRank())
                    {
                        Card temp = cards[i + 1];
                        cards[i + 1] = cards[i];
                        cards[i] = temp;
                        done = false;
                    }
                }
            }
        }
    }
}
