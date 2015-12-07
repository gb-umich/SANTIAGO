namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;

    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public interface ISimPlayerActionValidator
    {
        bool IsValid(SimPlayerAction action, SimPlayerTurnContext context, ICollection<Card> playerCards);

        ICollection<Card> GetPossibleCardsToPlay(SimPlayerTurnContext context, ICollection<Card> playerCards);
    }
}
