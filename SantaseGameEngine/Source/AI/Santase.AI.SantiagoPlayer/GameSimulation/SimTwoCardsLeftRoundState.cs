namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    public class SimTwoCardsLeftRoundState : SimBaseRoundState
    {
        public SimTwoCardsLeftRoundState(ISimStateManager round)
            : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => true;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => false;

        public override bool ShouldDrawCard => true;

        internal override void PlayHand(int cardsLeftInDeck)
        {
            this.Round.SetState(new SimFinalRoundState(this.Round));
        }
    }
}
