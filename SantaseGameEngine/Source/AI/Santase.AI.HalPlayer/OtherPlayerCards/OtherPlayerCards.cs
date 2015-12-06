namespace Santase.AI.HalPlayer.OtherPlayerCards
{                      
    using System.Collections.Generic;

    using Santase.Logic.Cards;

    public class OtherPlayerCards
    {
        public ICollection<Card> GetOtherPlayerCards(IDictionary<int, Card> myCards,
            IDictionary<int, Card> playedCards, Card activeTrumpCard, CardSuit suit)
        {
            var otherPlayerCards = new CardCollection
                                  {
                                      new Card(suit, CardType.Nine),
                                      new Card(suit, CardType.Jack),
                                      new Card(suit, CardType.Queen),
                                      new Card(suit, CardType.King),
                                      new Card(suit, CardType.Ten),
                                      new Card(suit, CardType.Ace),
                                  };

            foreach (var card in myCards)
            {
                if (card.Value.Suit == suit)
                {
                    otherPlayerCards.Remove(card.Value);
                }
            }

            foreach (var card in playedCards)
            {
                if (card.Value.Suit == suit)
                {
                    otherPlayerCards.Remove(card.Value);
                }
            }

            if (activeTrumpCard != null)
            {
                otherPlayerCards.Remove(activeTrumpCard);
            }

            return otherPlayerCards;
        }
    }
}
