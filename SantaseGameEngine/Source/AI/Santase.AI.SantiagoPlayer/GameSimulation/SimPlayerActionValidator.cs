namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System;
    using System.Collections.Generic;

    using Santase.Logic.Cards;
    using Logic.PlayerActionValidate;
    using Santase.Logic.Players;

    public class SimPlayerActionValidator : ISimPlayerActionValidator
    {
        private static readonly Lazy<SimPlayerActionValidator> Lazy =
            new Lazy<SimPlayerActionValidator>(() => new SimPlayerActionValidator());

        private readonly AnnounceValidator announceValidator = new AnnounceValidator();

        public static SimPlayerActionValidator Instance => Lazy.Value;

        public bool IsValid(SimPlayerAction action, SimPlayerTurnContext context, ICollection<Card> playerCards)
        {
            if (context.State.CanAnnounce20Or40)
            {
                action.Announce = this.announceValidator.GetPossibleAnnounce(
                    playerCards,
                    action.Card,
                    context.TrumpCard,
                    context.IsFirstPlayerTurn);
            }

            if (action == null)
            {
                return false;
            }

            if (action.Type == PlayerActionType.PlayCard)
            {
                var canPlayCard = SimPlayCardActionValidator.CanPlayCard(
                    context.IsFirstPlayerTurn,
                    action.Card,
                    context.FirstPlayedCard,
                    context.TrumpCard,
                    playerCards,
                    context.State.ShouldObserveRules);
                return canPlayCard;
            }

            if (action.Type == PlayerActionType.ChangeTrump)
            {
                var canChangeTrump = SimChangeTrumpActionValidator.CanChangeTrump(
                    context.IsFirstPlayerTurn,
                    context.State,
                    context.TrumpCard,
                    playerCards);
                return canChangeTrump;
            }

            // action.Type == PlayerActionType.CloseGame
            var canCloseGame = SimCloseGameActionValidator.CanCloseGame(context.IsFirstPlayerTurn, context.State);
            return canCloseGame;
        }

        public ICollection<Card> GetPossibleCardsToPlay(SimPlayerTurnContext context, ICollection<Card> playerCards)
        {
            var possibleCardsToPlay = new List<Card>(playerCards.Count);

            // ReSharper disable once LoopCanBeConvertedToQuery (performance)
            foreach (var card in playerCards)
            {
                if (SimPlayCardActionValidator.CanPlayCard(
                    context.IsFirstPlayerTurn,
                    card,
                    context.FirstPlayedCard,
                    context.TrumpCard,
                    playerCards,
                    context.State.ShouldObserveRules))
                {
                    possibleCardsToPlay.Add(card);
                }
            }

            return possibleCardsToPlay;
        }
    }
}
