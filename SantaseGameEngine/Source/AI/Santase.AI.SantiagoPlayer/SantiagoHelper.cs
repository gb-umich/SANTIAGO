namespace Santase.AI.SantiagoPlayer
{
    using System.Collections.Generic;
    using System.Linq;
    using GameSimulation;
    using Logic.Extensions;
    using Santase.Logic;
    using Santase.Logic.Cards;

    internal class SantiagoHelper : SimBasePlayer
    {
        private const int MaxPoints = 66;
        private const int HalfPoints = 33;
        private const int NumberOfPlayouts = 100;

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
                var currentContext = context.DeepClone();

                this.Cards = new List<Card>(cards);
                var currentDeck = new SimDeck(cardsNotInDeck, currentContext.TrumpCard);

                var firstAction = (currentContext.FirstPlayedCard != null) ? SimPlayerAction.PlayCard(currentContext.FirstPlayedCard) : null;
                var secondAction = (card == null) ? SimPlayerAction.CloseGame() : SimPlayerAction.PlayCard(card);

                var playAction = (card == null) ? this.CloseGame() : this.PlayCard(card);

                var firstPlayer = new SimDummyPlayer();

                var opponentCardsNumber = (currentContext.FirstPlayedCard == null) ? this.Cards.Count + 1 : this.Cards.Count;

                for (int i = 0; i < opponentCardsNumber; i++)
                {
                    if (currentDeck.CardsLeft == 0)
                    {
                        break;
                    }

                    firstPlayer.AddCard(currentDeck.GetNextCard());
                }

                var stateManager = new SimStateManager();
                stateManager.SetState(currentContext.State);
                var round = new SimRound(firstPlayer, this, GameRulesProvider.Santase, currentDeck, stateManager);


                var result = round.PlaySimulation(
                                                    currentContext.FirstPlayerRoundPoints,
                                                    currentContext.SecondPlayerRoundPoints,
                                                    firstAction,
                                                    secondAction,
                                                    firstPlayer.Cards,
                                                    this.Cards);

                value += this.Expand(result);
            }

            value = value / NumberOfPlayouts;

            return value;
        }

        private float Expand(SimRoundResult result)
        {
            if (result.SecondPlayer.RoundPoints >= MaxPoints)
            {
                if (result.FirstPlayer.RoundPoints < HalfPoints)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else if (result.FirstPlayer.RoundPoints >= MaxPoints)
            {
                if (result.SecondPlayer.RoundPoints < HalfPoints)
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
