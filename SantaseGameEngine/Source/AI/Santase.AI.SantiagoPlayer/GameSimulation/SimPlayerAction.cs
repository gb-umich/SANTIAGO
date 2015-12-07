namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using Logic;
    using Logic.Players;
    using Santase.Logic.Cards;

    public sealed class SimPlayerAction
        {
            private SimPlayerAction(PlayerActionType type, Card card)
            {
                this.Type = type;
                this.Card = card;
                this.Announce = Announce.None;
            }

            public PlayerActionType Type { get; }

            public Card Card { get; }

            internal Announce Announce { get; set; }

            public static SimPlayerAction PlayCard(Card card)
            {
                return new SimPlayerAction(PlayerActionType.PlayCard, card);
            }

            public static SimPlayerAction ChangeTrump()
            {
                return new SimPlayerAction(PlayerActionType.ChangeTrump, null);
            }

            public static SimPlayerAction CloseGame()
            {
                return new SimPlayerAction(PlayerActionType.CloseGame, null);
            }

            public override string ToString()
            {
                return $"Action: {this.Type}; Card: {this.Card}; Announce: {this.Announce}";
            }
        }
    }