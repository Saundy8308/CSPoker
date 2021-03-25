using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    // Pack represents a standard pack of 52 cards
    // it creates and frees the Card objects
    public class Pack
    {

        Random rnd = new Random();
        private Card[] cardsArray = new Card[52];
        private int top;
        private int bottom;
        private int size;
        // size is number of cards currently in pack
        public int Size
        {
            get
            {
                return size;
            }
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public bool IsFull()
        {
            return size == 52;
        }

        // constructor
        public Pack()
        {
            for (int i = 0; i < 52; i++)
            {
                cardsArray[i] = new Card(i % 13 + 1, i / 13 + 1);
            }
            //!!!!!!!
            // initialise the three integers below
            top = 0;
            bottom = 51;
            size = 52;
        }

        public void Shuffle()
        {
            for (int i = top; i <= bottom; i++)
            {
                int r = rnd.Next(i, bottom + 1);
                Card temp = cardsArray[i];
                cardsArray[i] = cardsArray[r];
                cardsArray[r] = temp;

            }
        }

        public Card DealCard()
        {
            if (!IsEmpty())
            {
                Card ACard = cardsArray[top];
                if (top == 51)
                    top = 0;
                else
                    top++;
                size--;
                return ACard;
            }
            else
                return null;
        }

        public void AddCard(Card ACard)
        {
            if (!IsFull())
            {
                if (bottom == 51)
                {
                    bottom = 0;
                }
                else
                {
                    bottom++;
                }
                cardsArray[bottom] = ACard;
                size++;
            }

        }

        public void AddCard(Hand cards)
        {
            for (int i = 0; i < cards.Size; i++)
            {
                AddCard(cards[i]);
            }
        }
    }
}
