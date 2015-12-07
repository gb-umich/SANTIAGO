namespace Santase.AI.SantiagoPlayer
{
    using System.Linq;
    using GameSimulation;
    using Santase.Logic.Extensions;

    public class SimDummyPlayer : SimBasePlayer
    {
        public SimDummyPlayer(string name = "Dummy")
        {
            this.Name = name;
        }

        public override string Name { get; }

        public override SimPlayerAction GetTurn(SimPlayerTurnContext context)
        {
            var validCardsToPlay = this.PlayerActionValidator.GetPossibleCardsToPlay(context, this.Cards);
            var randomCard = validCardsToPlay.Shuffle().First();

            return this.PlayCard(randomCard);
        }
    }
}
