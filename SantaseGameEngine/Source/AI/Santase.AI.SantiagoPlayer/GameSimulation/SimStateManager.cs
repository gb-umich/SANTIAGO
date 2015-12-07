namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    public class SimStateManager : ISimStateManager
    {
        public SimStateManager()
        {
            this.State = new SimStartRoundState(this);
        }

        public SimBaseRoundState State { get; private set; }
        
        public void SetState(SimBaseRoundState newState)
        {
            this.State = newState;
        }
    }
}
