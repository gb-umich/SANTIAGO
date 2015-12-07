namespace Santase.AI.SantiagoPlayer
{
    using System.Collections.Generic;

    using GameSimulation;
    using Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public class SantiagoPlayer : BasePlayer
    {
        public SantiagoPlayer()
        {
            this.CardsNotInDeck = new HashSet<Card>();
            this.MonteCarlo = new Dictionary<uint, Card>();
        }

        public override string Name
        {
            get
            {
                return "Santiago Player";
            }
        }

        private HashSet<Card> CardsNotInDeck { get; set; }

        private Dictionary<uint, Card> MonteCarlo { get; set; }

        public override PlayerAction GetTurn(PlayerTurnContext context)
        {
            //if (this.PlayerActionValidator.IsValid(PlayerAction.ChangeTrump(), context, this.Cards))
            //{
            //    this.CardsNotInDeck.Add(context.TrumpCard);

            //    this.CardsNotInDeck.RemoveWhere(c => c.Suit == context.TrumpCard.Suit && c.Type == CardType.Nine);

            //    return this.ChangeTrump(context.TrumpCard);
            //}

            //foreach (var card in this.Cards)
            //{
            //    if (card.Type == CardType.King &&
            //        (this.AnnounceValidator.GetPossibleAnnounce(this.Cards, card, context.TrumpCard) == Announce.Forty ||
            //         this.AnnounceValidator.GetPossibleAnnounce(this.Cards, card, context.TrumpCard) == Announce.Twenty))
            //    {
            //        this.Cards.Remove(card);
            //        return this.PlayCard(card);
            //    }
            //}

            var bestCard = this.GetCardToPlay(context);

            return this.PlayCard(bestCard);
        }

        public override void EndTurn(PlayerTurnContext context)
        {
            if (context.FirstPlayedCard != null)
            {
                this.CardsNotInDeck.Add(context.FirstPlayedCard);
            }

            if (context.SecondPlayedCard != null)
            {
                this.CardsNotInDeck.Add(context.SecondPlayedCard);
            }
        }

        public override void EndRound()
        {
            this.CardsNotInDeck.Clear();
            base.EndRound();
        }

        private static SimPlayerTurnContext GetCurrentContext(PlayerTurnContext context)
        {
            SimPlayerTurnContext currentContext = null;

            var stateManager = new SimStateManager();

            if (context.State.GetType().Name == "StartRoundState")
            {
                var state = new SimStartRoundState(stateManager);

                currentContext = new SimPlayerTurnContext(state, context.TrumpCard, context.CardsLeftInDeck, context.FirstPlayerRoundPoints, context.SecondPlayerRoundPoints);
            }
            else if (context.State.GetType().Name == "TwoCardsLeftRoundState")
            {
                var state = new SimTwoCardsLeftRoundState(stateManager);

                currentContext = new SimPlayerTurnContext(state, context.TrumpCard, context.CardsLeftInDeck, context.FirstPlayerRoundPoints, context.SecondPlayerRoundPoints);
            }
            else if (context.State.GetType().Name == "MoreThanTwoCardsLeftRoundState")
            {
                var state = new SimMoreThanTwoCardsLeftRoundState(stateManager);

                currentContext = new SimPlayerTurnContext(state, context.TrumpCard, context.CardsLeftInDeck, context.FirstPlayerRoundPoints, context.SecondPlayerRoundPoints);
            }
            else if (context.State.GetType().Name == "FinalRoundState")
            {
                var state = new SimFinalRoundState(stateManager);

                currentContext = new SimPlayerTurnContext(state, context.TrumpCard, context.CardsLeftInDeck, context.FirstPlayerRoundPoints, context.SecondPlayerRoundPoints);
            }

            currentContext.FirstPlayedCard = context.FirstPlayedCard;
            currentContext.SecondPlayedCard = context.SecondPlayedCard;
            currentContext.FirstPlayerAnnounce = context.FirstPlayerAnnounce;

            return currentContext;
        }

        private Card GetCardToPlay(PlayerTurnContext context)
        {
            foreach (var card in this.Cards)
            {
                if (!this.CardsNotInDeck.Contains(card))
                {
                    this.CardsNotInDeck.Add(card);
                }
            }

            if (context.FirstPlayedCard != null)
            {
                this.CardsNotInDeck.Add(context.FirstPlayedCard);
            }

            if (context.SecondPlayedCard != null)
            {
                this.CardsNotInDeck.Add(context.SecondPlayedCard);
            }

            var validCards = this.GetValidCards(context);

            var hashedHand = this.GetHash(this.Cards, this.CardsNotInDeck);

            if (this.MonteCarlo.ContainsKey(hashedHand))
            {
                for (int i = 0; i < validCards.Count; i++)
                {
                    var cardHash = this.GetTwoNumberHash((uint)validCards[i].Type, (uint)validCards[i].Suit);

                    if (validCards[i] == this.MonteCarlo[hashedHand])
                    {
                        return validCards[i];
                    }
                }
            }

            float bestActionProbability = float.MinValue;
            int bestActionIndex = 0;

            var currentContext = GetCurrentContext(context);
            var helper = new SantiagoHelper(this.Cards);
            for (int i = 0; i < validCards.Count; i++)
            {
                var probability = helper.GetProbability(currentContext, validCards[i], this.CardsNotInDeck, this.Cards);

                if (probability >= bestActionProbability)
                {
                    bestActionProbability = probability;
                    bestActionIndex = i;
                }
            }

            this.MonteCarlo[hashedHand] = validCards[bestActionIndex];

            return validCards[bestActionIndex];
        }

        private List<Card> GetValidCards(PlayerTurnContext context)
        {
            List<Card> validCards = new List<Card>();

            foreach (var card in this.Cards)
            {
                if (this.PlayerActionValidator.IsValid(PlayerAction.PlayCard(card), context, this.Cards))
                {
                    validCards.Add(card);
                }
            }

            return validCards;
        }

        private uint GetHash(ICollection<Card> playerCards, HashSet<Card> notInDeck)
        {
            uint playerHash = 0;
            foreach (var card in playerCards)
            {
                var hashed = this.GetTwoNumberHash((uint)card.Type, (uint)card.Suit);
                var reversed = this.ReverseBits(hashed);

                playerHash += reversed;
            }

            uint notInDeckHash = 0;
            foreach (var card in notInDeck)
            {
                var hashed = this.GetTwoNumberHash((uint)card.Type, (uint)card.Suit);
                var reversed = this.ReverseBits(hashed);

                notInDeckHash += reversed;
            }

            return this.GetTwoNumberHash(playerHash, notInDeckHash);
        }

        private uint GetTwoNumberHash(uint n1, uint n2)
        {
            var result = ((n1 + n2) * (n1 + n2 + 1) + n2) / 2;

            return result;
        }

        private uint ReverseBits(uint x)
        {
            uint y = 0;
            for (int i = 0; i < 32; ++i)
            {
                y <<= 1;
                y |= x & 1;
                x >>= 1;
            }

            return y;
        }
    }
}