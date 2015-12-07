namespace Santase.AI.SantiagoPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logic.Extensions;
    using GameSimulation;
    using Santase.Logic;
    using Santase.Logic.Cards;

    internal class SantiagoHelper : SimBasePlayer
    {
        public const int NumberOfPlayouts = 50;

        //public static Dictionary<CardType, int> CardRank = new Dictionary<CardType, int>()
        //{
        //    { CardType.Nine, 1 },
        //    { CardType.Jack, 2 },
        //    { CardType.Queen, 3 },
        //    { CardType.King, 4 },
        //    { CardType.Ten, 5 },
        //    { CardType.Ace, 6 },
        //};

        public SantiagoHelper()
        {
        }

        public SantiagoHelper(ICollection<Card> cards)
            : this()
        {
            foreach (var c in cards)
            {
                this.Cards.Add(c);
            }
        }

        public override string Name
        {
            get
            {
                return "Santiago Helper";
            }
        }

        public override SimPlayerAction GetTurn(SimPlayerTurnContext context)
        {
            var validCardsToPlay = this.PlayerActionValidator.GetPossibleCardsToPlay(context, this.Cards);
            var randomCard = validCardsToPlay.Shuffle().First();

            return this.PlayCard(randomCard);
        }

        public float GetProbability(SimPlayerTurnContext context, Card card, HashSet<Card> cardsNotInDeck, ICollection<Card> cards)
        {
            float value = 0;
            for (int play = 0; play < NumberOfPlayouts; ++play)
            {
                this.Cards = new List<Card>(cards);
                var currentDeck = new SimDeck(cardsNotInDeck, context.TrumpCard);

                var firstAction = (context.FirstPlayedCard != null) ? SimPlayerAction.PlayCard(context.FirstPlayedCard) : null;
                var secondAction = (card == null) ? SimPlayerAction.CloseGame() : SimPlayerAction.PlayCard(card);

                var playAction = (card == null) ? this.CloseGame() : this.PlayCard(card);


                var firstPlayer = new SimDummyPlayer();

                var opponentCardsNumber = (context.FirstPlayedCard == null) ? this.Cards.Count + 1 : this.Cards.Count;

                for (int i = 0; i < opponentCardsNumber; i++)
                {
                    firstPlayer.AddCard(currentDeck.GetNextCard());
                }

                var stateManager = new SimStateManager();
                stateManager.SetState(context.State);
                var round = new SimRound(firstPlayer, this, GameRulesProvider.Santase, currentDeck, stateManager);

                var currentContext = context.DeepClone();

                var result = round.PlaySimulation(
                                                    context.FirstPlayerRoundPoints,
                                                    context.SecondPlayerRoundPoints,
                                                    firstAction,
                                                    secondAction,
                                                    firstPlayer.Cards,
                                                    this.Cards);

                value += Expand(result);
            }

            value = value / NumberOfPlayouts;

            return value;
        }

        private float Expand(SimRoundResult result)
        {
            if (result.SecondPlayer.RoundPoints >= 66)
            {
                if (result.FirstPlayer.RoundPoints < 33)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else if (result.FirstPlayer.RoundPoints >= 66)
            {
                if (result.SecondPlayer.RoundPoints < 33)
                {
                    return -2;
                }
                else
                {
                    return -1;
                }
            }
            else if (result.FirstPlayer.GameCloser)
            {
                return 3;
            }
            else if (result.SecondPlayer.GameCloser)
            {
                return -3;
            }
            else if (result.LastWinner.Player == this)
            {
                return 1;
            }

            return -1;
        }
    }
}
