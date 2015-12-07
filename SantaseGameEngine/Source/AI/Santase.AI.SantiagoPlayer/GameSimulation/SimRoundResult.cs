namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using Santase.Logic;

    internal class SimRoundResult
        {
            public SimRoundResult(SimRoundPlayerInfo firstPlayer, SimRoundPlayerInfo secondPlayer)
            {
                this.FirstPlayer = firstPlayer;
                this.SecondPlayer = secondPlayer;
            }

        public SimRoundResult(SimRoundPlayerInfo firstPlayer, SimRoundPlayerInfo secondPlayer, SimRoundPlayerInfo lastWinningPlayer)
            : this(firstPlayer, secondPlayer)
        {
            this.FirstPlayer = firstPlayer;
            this.SecondPlayer = secondPlayer;
            this.LastWinner = lastWinningPlayer;
        }

        public SimRoundPlayerInfo FirstPlayer { get; }

            public SimRoundPlayerInfo SecondPlayer { get; }

            public PlayerPosition GameClosedBy
            {
                get
                {
                    if (this.FirstPlayer.GameCloser)
                    {
                        return PlayerPosition.FirstPlayer;
                    }

                    if (this.SecondPlayer.GameCloser)
                    {
                        return PlayerPosition.SecondPlayer;
                    }

                    return PlayerPosition.NoOne;
                }
            }

            public PlayerPosition NoTricksPlayer
            {
                get
                {
                    if (!this.FirstPlayer.HasAtLeastOneTrick)
                    {
                        return PlayerPosition.FirstPlayer;
                    }

                    if (!this.SecondPlayer.HasAtLeastOneTrick)
                    {
                        return PlayerPosition.SecondPlayer;
                    }

                    return PlayerPosition.NoOne;
                }
            }

        public SimRoundPlayerInfo LastWinner { get; private set; }
    }
    }

