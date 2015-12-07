namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    // https://github.com/NikolayIT/SantaseGameEngine/blob/master/Documentation/Rules.md#the-play
    public class SimFinalRoundState : SimBaseRoundState
    {
        public SimFinalRoundState(ISimStateManager round)
            : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => true;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => true;

        public override bool ShouldDrawCard => false;

        internal override void PlayHand(int cardsLeftInDeck)
        {
        }
    }
}
