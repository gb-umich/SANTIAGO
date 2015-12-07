# Santase Game Engine AI
_____________________________________

**Santase** (known as **66**, Сантасе, Sixty-six or **Sechsundsechzig**) is a well-known card game in Bulgaria and also played in Germany.

It is a fast **6-card game** for **2 players** played with a deck of 24 cards consisting of the _Ace_, _Ten_, _King_, _Queen_, _Jack_ and _Nine_.

#### Monte Carlo Tree Search

- **Monte Carlo Tree Search** is a general heuristic algorithm that does not require game specific knowledge and can be used for games with high-branching factor (with some optimization the complexity is ~ ``O(log(n))``). 

- It does not give a perfect solution, but rather an approximation. It can be adapted for a lot of games where there is a certain amount of randomness.

- The **Monte Carlo** method works by running a large number of random simulations on several sets of initial conditions and picking out the one which is most often successful.

It works well for **Santase AI** bot. For every trick we run a number of simulations for all valid moves -- we play the initial move and for the rest of the round we choose moves at random. This yields a measure of the success rate of the particular initial move.

The result of running the simulations can be structured in a tree that guides every step of the play by the previously calculated probabilities. The only downside is that initially the algorithm must play at random until the tree is built. There is a well-known optimisation called **Upper Confidence bound for Trees (UCT)** that balances between exploring new paths and concentrating on the current best solutions, for instance by assigning _confidence intervals_ for each valid move.

The AI player in our particular implementation uses a helper player that plays a set of simulations for all the possible moves at a given stage of the game. It then returns an average success rate, from which the player chooses the best action to take and saves the current move to use later when needed, i.e. for each step we have best action to take and for unvisited paths we make an expansion and record it.
