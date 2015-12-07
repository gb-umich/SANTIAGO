namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;

    using Santase.Logic.Cards;
    using Santase.Logic.RoundStates;

    internal static class SimChangeTrumpActionValidator
    {
        public static bool CanChangeTrump(bool isThePlayerFirst, SimBaseRoundState state, Card trumpCard, ICollection<Card> playerCards)
        {
            if (!isThePlayerFirst)
            {
                return false;
            }

            if (!state.CanChangeTrump)
            {
                return false;
            }

            return playerCards.Contains(new Card(trumpCard.Suit, CardType.Nine));
        }
    }
}
