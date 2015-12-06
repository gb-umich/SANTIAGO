namespace Santase.AI.HalPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public class HalPlayer : BasePlayer
    {
        private readonly IDictionary<int, Card> playedCards = new Dictionary<int, Card>();

        private readonly OtherPlayerCards.OtherPlayerCards otherPlayerCardsProvider = new OtherPlayerCards.OtherPlayerCards();

        public HalPlayer(string name = "Hal Player")
        {
            this.Name = name;
        }

        public override string Name { get; }

        public override PlayerAction GetTurn(PlayerTurnContext context)
        {
            return this.NextCardToPlay(context);
        }

        public override void EndRound()
        {
            this.playedCards.Clear();
            base.EndRound();
        }

        public override void EndTurn(PlayerTurnContext context)
        {
            if (!this.playedCards.ContainsKey(context.FirstPlayedCard.GetHashCode()))
            {
                this.playedCards[context.FirstPlayedCard.GetHashCode()] = 
                    new Card(context.FirstPlayedCard.Suit, context.FirstPlayedCard.Type);
            }

            this.playedCards[context.FirstPlayedCard.GetHashCode()] = context.FirstPlayedCard;

            if (!this.playedCards.ContainsKey(context.SecondPlayedCard.GetHashCode()))
            {
                this.playedCards[context.SecondPlayedCard.GetHashCode()] =
                    new Card(context.SecondPlayedCard.Suit, context.SecondPlayedCard.Type);
            }

            this.playedCards[context.SecondPlayedCard.GetHashCode()] = context.SecondPlayedCard;
        }


        private PlayerAction NextCardToPlay(PlayerTurnContext context)
        {
            return null;
        }
    }
}
