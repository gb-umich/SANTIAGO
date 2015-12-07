namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;
    using Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using Santase.Logic.RoundStates;

    internal class SimRound
    {
        private readonly IGameRules gameRules;

        private readonly IDeck deck;

        private readonly ISimStateManager stateManager;

        private readonly SimRoundPlayerInfo firstPlayer;

        private readonly SimRoundPlayerInfo secondPlayer;

        private PlayerPosition firstToPlay;

        public SimRound(
            ISimPlayer firstPlayer,
            ISimPlayer secondPlayer,
            IGameRules gameRules,
            PlayerPosition firstToPlay = PlayerPosition.FirstPlayer)
        {
            this.gameRules = gameRules;
            this.deck = new SimDeck();
            this.stateManager = new SimStateManager();

            this.firstPlayer = new SimRoundPlayerInfo(firstPlayer);
            this.secondPlayer = new SimRoundPlayerInfo(secondPlayer);

            this.firstToPlay = firstToPlay;
        }

        public SimRound(
                ISimPlayer firstPlayer,
                ISimPlayer secondPlayer,
                IGameRules gameRules,
                IDeck deck,
                ISimStateManager stateManager,
                PlayerPosition firstToPlay = PlayerPosition.FirstPlayer)
        {
            this.gameRules = gameRules;
            this.deck = deck;
            this.stateManager = new SimStateManager(); //stateManager;

            this.firstPlayer = new SimRoundPlayerInfo(firstPlayer);
            this.secondPlayer = new SimRoundPlayerInfo(secondPlayer);

            this.firstToPlay = firstToPlay;
        }

        public SimRoundResult PlaySimulation(
            int firstPlayerTotalPoints,
            int secondPlayerTotalPoints,
            SimPlayerAction firstAction,
            SimPlayerAction secondAction,
            ICollection<Card> firstPlayerCards,
            ICollection<Card> secondPlayerCards)
        {
            this.CallStartRoundAndDealCardsSimulation(this.firstPlayer, firstPlayerTotalPoints, secondPlayerTotalPoints, firstPlayerCards);
            this.CallStartRoundAndDealCardsSimulation(this.secondPlayer, secondPlayerTotalPoints, firstPlayerTotalPoints, secondPlayerCards);

            SimRoundPlayerInfo lastTrickWinner;

            lastTrickWinner = this.PlayFirstTrickSimulation(firstAction, secondAction);

            while (!this.IsFinished())
            {
                if (this.deck.CardsLeft == 1)
                {
                    break;
                }

                lastTrickWinner = this.PlayTrickSimulation();
            }

            this.firstPlayer.Player.EndRound();
            this.secondPlayer.Player.EndRound();

            return new SimRoundResult(this.firstPlayer, this.secondPlayer, lastTrickWinner);
        }

        public SimRoundResult Play(int firstPlayerTotalPoints, int secondPlayerTotalPoints)
        {
            this.CallStartRoundAndDealCards(this.firstPlayer, firstPlayerTotalPoints, secondPlayerTotalPoints);
            this.CallStartRoundAndDealCards(this.secondPlayer, secondPlayerTotalPoints, firstPlayerTotalPoints);

            while (!this.IsFinished())
            {
                this.PlayTrick();
            }

            this.firstPlayer.Player.EndRound();
            this.secondPlayer.Player.EndRound();

            return new SimRoundResult(this.firstPlayer, this.secondPlayer);
        }

        public SimRoundPlayerInfo PlayFirstTrickSimulation(SimPlayerAction firstAction, SimPlayerAction secondAction)
        {
            var trick = this.firstToPlay == PlayerPosition.FirstPlayer
                ? new SimTrick(this.firstPlayer, this.secondPlayer, this.stateManager, this.deck, this.gameRules)
                : new SimTrick(this.secondPlayer, this.firstPlayer, this.stateManager, this.deck, this.gameRules);

            var trickWinner = trick.PlayFirstSimulation(firstAction, secondAction);

            // The one who wins the trick should play first
            this.firstToPlay = trickWinner == this.firstPlayer
                                   ? PlayerPosition.FirstPlayer
                                   : PlayerPosition.SecondPlayer;

            if (this.stateManager.State.ShouldDrawCard)
            {
                // The player who wins last trick takes card first
                if (this.deck.CardsLeft > 0)
                {
                    if (this.firstToPlay == PlayerPosition.FirstPlayer)
                    {
                        this.GiveCardToPlayer(this.firstPlayer);
                        this.GiveCardToPlayer(this.secondPlayer);
                    }
                    else
                    {
                        this.GiveCardToPlayer(this.secondPlayer);
                        this.GiveCardToPlayer(this.firstPlayer);
                    }
                }
            }

            this.stateManager.State.PlayHand(this.deck.CardsLeft);

            return trickWinner;
        }

        public SimRoundPlayerInfo PlayTrickSimulation()
        {
            var trick = this.firstToPlay == PlayerPosition.FirstPlayer
                ? new SimTrick(this.firstPlayer, this.secondPlayer, this.stateManager, this.deck, this.gameRules)
                : new SimTrick(this.secondPlayer, this.firstPlayer, this.stateManager, this.deck, this.gameRules);

            var trickWinner = trick.Play();

            // The one who wins the trick should play first
            this.firstToPlay = trickWinner == this.firstPlayer
                                   ? PlayerPosition.FirstPlayer
                                   : PlayerPosition.SecondPlayer;

            if (this.stateManager.State.ShouldDrawCard)
            {
                // The player who wins last trick takes card first
                if (this.deck.CardsLeft > 0)
                {
                    if (this.firstToPlay == PlayerPosition.FirstPlayer)
                    {
                        this.GiveCardToPlayer(this.firstPlayer);
                        this.GiveCardToPlayer(this.secondPlayer);
                    }
                    else
                    {
                        this.GiveCardToPlayer(this.secondPlayer);
                        this.GiveCardToPlayer(this.firstPlayer);
                    }
                }
            }

            this.stateManager.State.PlayHand(this.deck.CardsLeft);

            return trickWinner;
        }

        private void PlayTrick()
        {
            var trick = this.firstToPlay == PlayerPosition.FirstPlayer
                ? new SimTrick(this.firstPlayer, this.secondPlayer, this.stateManager, this.deck, this.gameRules)
                : new SimTrick(this.secondPlayer, this.firstPlayer, this.stateManager, this.deck, this.gameRules);

            var trickWinner = trick.Play();

            // The one who wins the trick should play first
            this.firstToPlay = trickWinner == this.firstPlayer
                                   ? PlayerPosition.FirstPlayer
                                   : PlayerPosition.SecondPlayer;

            if (this.stateManager.State.ShouldDrawCard)
            {
                // The player who wins last trick takes card first
                if (this.firstToPlay == PlayerPosition.FirstPlayer)
                {
                    this.GiveCardToPlayer(this.firstPlayer);
                    this.GiveCardToPlayer(this.secondPlayer);
                }
                else
                {
                    this.GiveCardToPlayer(this.secondPlayer);
                    this.GiveCardToPlayer(this.firstPlayer);
                }
            }

            this.stateManager.State.PlayHand(this.deck.CardsLeft);
        }

        public bool IsFinished()
        {
            if (this.firstPlayer.RoundPoints >= this.gameRules.RoundPointsForGoingOut)
            {
                return true;
            }

            if (this.secondPlayer.RoundPoints >= this.gameRules.RoundPointsForGoingOut)
            {
                return true;
            }

            if (this.deck.CardsLeft == 0)
            {
                return true;
            }

            // No cards left => round over
            return this.firstPlayer.Cards.Count == 0 && this.secondPlayer.Cards.Count == 0;
        }

        private void CallStartRoundAndDealCardsSimulation(SimRoundPlayerInfo player, int playerTotalPoints, int opponentTotalPoints, ICollection<Card> currentCards)
        {
            var cards = new List<Card>();

            // TODO: 6 should be constant
            foreach (var card in currentCards)
            {
                cards.Add(card);
                player.Cards.Add(card);
            }

            player.Player.StartRound(cards, this.deck.TrumpCard, playerTotalPoints, opponentTotalPoints);
        }

        private void CallStartRoundAndDealCards(SimRoundPlayerInfo player, int playerTotalPoints, int opponentTotalPoints)
        {
            var cards = new List<Card>();

            // TODO: 6 should be constant
            for (var i = 0; i < 6; i++)
            {
                if (this.deck.CardsLeft == 0)
                {
                    break;
                }

                var card = this.deck.GetNextCard();
                cards.Add(card);
                player.Cards.Add(card);
            }

            player.Player.StartRound(cards, this.deck.TrumpCard, playerTotalPoints, opponentTotalPoints);
        }

        private void GiveCardToPlayer(SimRoundPlayerInfo player)
        {
            var card = this.deck.GetNextCard();
            player.AddCard(card);
        }
    }
}

