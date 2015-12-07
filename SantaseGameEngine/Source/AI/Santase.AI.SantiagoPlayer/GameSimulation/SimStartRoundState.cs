namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    public class SimStartRoundState : SimBaseRoundState
    {
        public SimStartRoundState(ISimStateManager round)
            : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => false;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => false;

        public override bool ShouldDrawCard => true;

        internal override void PlayHand(int cardsLeftInDeck)
        {
            this.Round.SetState(new SimMoreThanTwoCardsLeftRoundState(this.Round));
        }
    }
}
