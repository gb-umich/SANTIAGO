﻿namespace Santase.AI.SantiagoPlayer.GameSimulation
{
    using System.Collections.Generic;
    using Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    internal class SimRoundPlayerInfo
        {
            public SimRoundPlayerInfo(ISimPlayer player)
            {
                this.Player = player;
                this.Cards = new CardCollection();
                this.TrickCards = new List<Card>();
                this.Announces = new List<Announce>();
                this.GameCloser = false;
            }

            public ISimPlayer Player { get; }

            public ICollection<Card> Cards { get; }

            public ICollection<Card> TrickCards { get; }

            public ICollection<Announce> Announces { get; }

            public bool GameCloser { get; set; }

            public int RoundPoints
            {
                get
                {
                    var points = 0;

                    // ReSharper disable once LoopCanBeConvertedToQuery (performance improvement)
                    foreach (var card in this.TrickCards)
                    {
                        points += card.GetValue();
                    }

                    // ReSharper disable once LoopCanBeConvertedToQuery (performance improvement)
                    foreach (var announce in this.Announces)
                    {
                        points += (int)announce;
                    }

                    return points;
                }
            }

            public bool HasAtLeastOneTrick => this.TrickCards.Count > 0;

            public void AddCard(Card card)
            {
                // We are adding the card in two different places to control what an AI player can play
                this.Cards.Add(card);
                this.Player.AddCard(card);
            }
        }
    }

