namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    public interface ISimStateManager
    {
        SimBaseRoundState State { get; }

        void SetState(SimBaseRoundState newState);
    }
}

