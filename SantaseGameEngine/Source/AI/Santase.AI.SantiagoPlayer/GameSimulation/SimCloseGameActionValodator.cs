namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    internal static class SimCloseGameActionValidator
    {
        public static bool CanCloseGame(bool isThePlayerFirst, SimBaseRoundState state)
        {
            return isThePlayerFirst && state.CanClose;
        }
    }
}
