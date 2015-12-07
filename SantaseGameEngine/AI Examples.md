1. Used method `GetHash()` for creating an unique hash of the played cards.
```cs
			uint playerHash = 0;
            foreach (var card in playerCards)
            {
                var hashed = this.GetTwoNumberHash((uint)card.Type, (uint)card.Suit);
                var reversed = this.ReverseBits(hashed);

                playerHash += reversed;
            }
```

2. Used support methods `GetTwoNumberHash()` and `ReverseBits()` for getting unique numbers.
```cs
		private uint GetTwoNumberHash(uint n1, uint n2)
        {
            var result = ((n1 + n2) * (n1 + n2 + 1) + n2) / 2;

            return result;
        }

        private uint ReverseBits(uint x)
        {
            uint y = 0;
            for (int i = 0; i < 32; ++i)
            {
                y <<= 1;
                y |= x & 1;
                x >>= 1;
            }

            return y;
        }
```

3. Used method `GetCurrentContext()` that takes the PlayerTurnContext of the game and creates SimPlayerTurnContext depending on the state of the game. First and second player cards corresponding to the state of the context are being copied as well.
```cs
			if (context.State.GetType().Name == "StartRoundState")
            {
                var state = new SimStartRoundState(stateManager);

                currentContext = new SimPlayerTurnContext(state, context.TrumpCard, context.CardsLeftInDeck, context.FirstPlayerRoundPoints, context.SecondPlayerRoundPoints);
            }
```

4. User a helper method that takes valid card and using simulation with dummy player returns the probability that card to be a good choice. Then Monte Carlo Tree Search algorithm finds the best probability, e.g. the best card to play.
```cs
			for (int i = 0; i < validCards.Count; i++)
            {
                var probability = helper.GetProbability(currentContext, validCards[i], this.CardsNotInDeck, this.Cards);

                if (probability >= bestActionProbability)
                {
                    bestActionProbability = probability;
                    bestActionIndex = i;
                }
            }
```