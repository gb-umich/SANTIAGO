namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;

    using Santase.Logic.Cards;
    using Santase.Logic.PlayerActionValidate;

    public abstract class SimBasePlayer : ISimPlayer
    {
        protected SimBasePlayer()
        {
            this.Cards = new CardCollection();
            this.AnnounceValidator = new AnnounceValidator();
            this.PlayerActionValidator = new SimPlayerActionValidator();
        }

        public abstract string Name { get; }

        public ICollection<Card> Cards { get; set; }

        protected IAnnounceValidator AnnounceValidator { get; }

        protected ISimPlayerActionValidator PlayerActionValidator { get; }

        public virtual void StartGame(string otherPlayerIdentifier)
        {
        }

        public virtual void StartRound(ICollection<Card> cards, Card trumpCard, int myTotalPoints, int opponentTotalPoints)
        {
            this.Cards.Clear();
            foreach (var card in cards)
            {
                this.Cards.Add(card);
            }
        }

        public virtual void AddCard(Card card)
        {
            this.Cards.Add(card);
        }

        public abstract SimPlayerAction GetTurn(SimPlayerTurnContext context);

        public virtual void EndTurn(SimPlayerTurnContext context)
        {
        }

        public virtual void EndRound()
        {
        }

        public virtual void EndGame(bool amIWinner)
        {
        }

        protected SimPlayerAction ChangeTrump(Card trumpCard)
        {
            this.Cards.Remove(new Card(trumpCard.Suit, CardType.Nine));
            this.Cards.Add(trumpCard);
            return SimPlayerAction.ChangeTrump();
        }

        protected SimPlayerAction PlayCard(Card card)
        {
            this.Cards.Remove(card);
            return SimPlayerAction.PlayCard(card);
        }

        protected SimPlayerAction CloseGame()
        {
            return SimPlayerAction.CloseGame();
        }
    }
}
