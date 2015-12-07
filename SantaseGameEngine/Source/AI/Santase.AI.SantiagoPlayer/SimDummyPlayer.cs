namespace Santase.AI.SantiagoPlayer
{
    using System.Linq;
    using Logic.Cards;
    using Logic;
    using Santase.Logic.Extensions;
    using Santase.Logic.Players;
    using GameSimulation;

    public class SimDummyPlayer : SimBasePlayer
    {
        public SimDummyPlayer(string name = "Dummy Player Lvl. 1")
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
