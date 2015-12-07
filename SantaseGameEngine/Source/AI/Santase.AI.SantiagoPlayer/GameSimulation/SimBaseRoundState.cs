namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    public abstract class SimBaseRoundState
        {
            protected SimBaseRoundState(ISimStateManager round)
            {
                this.Round = round;
            }

            public abstract bool CanAnnounce20Or40 { get; }

            public abstract bool CanClose { get; }

            public abstract bool CanChangeTrump { get; }

            public abstract bool ShouldObserveRules { get; }

            public abstract bool ShouldDrawCard { get; }

            protected ISimStateManager Round { get; }

            internal abstract void PlayHand(int cardsLeftInDeck);

            internal void Close()
            {
                if (this.CanClose)
                {
                    this.Round.SetState(new SimFinalRoundState(this.Round));
                }
            }
        }
    }
