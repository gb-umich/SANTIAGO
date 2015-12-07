namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;

    using Santase.Logic.Cards;

    public interface ISimPlayer
    {
        string Name { get; }

        ICollection<Card> Cards { get; }

        void StartGame(string otherPlayerIdentifier);

        void StartRound(ICollection<Card> cards, Card trumpCard, int myTotalPoints, int opponentTotalPoints);

        void AddCard(Card card);

        SimPlayerAction GetTurn(SimPlayerTurnContext context);

        void EndTurn(SimPlayerTurnContext context);

        void EndRound();

        void EndGame(bool amIWinner);
    }
}
