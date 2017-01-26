using MeesGame;
using System.Collections.Generic;
using System.Threading;

namespace AI
{
    /// <summary>
    /// An AI whose decisions are distributed uniformly across the set of possible actions.
    /// The AI will wait at least 10 ms between actions
    /// </summary>
    class RandomWalker : IAI
    {
        IAIPlayer player;
        PlayerAction nextAction = PlayerAction.NONE;
        PlayerAction lastAction = PlayerAction.NONE;

        public void GameStart(IAIPlayer player, int difficulty)
        {
            this.player = player;
        }

        public void ThinkAboutNextAction()
        {
            nextAction = PlayerAction.NONE;
            IPlayer dummy = player.DummyPlayer;

            //scientists have claimed sleeping helps you make better decisions
            //granted, it was only tested on humans, but it probably applies to computers as well
            Thread.Sleep(10);

            IList<PlayerAction> possibleActions = new List<PlayerAction>();
            // Pick a random action out of the possible actions
            foreach (PlayerAction action in dummy.PossibleActions)
            {
                if (action == PlayerAction.NONE) continue;
                IPlayer extraDummy = dummy.Clone();
                extraDummy.PerformAction(nextAction);
                if (extraDummy.CurrentTile is HoleTile) continue;
                possibleActions.Add(action);
            }
            if (possibleActions.Count != 1)
            {
                for (int i = possibleActions.Count - 1; i >= 0; i--)
                {
                    if (lastAction.ReverseAction() == possibleActions[i])
                        possibleActions.RemoveAt(i);
                }
            }
            nextAction = possibleActions[GameEnvironment.Random.Next(possibleActions.Count)];
        }

        public void UpdateNextAction()
        {
            if (player.DummyPlayer.CanPerformAction(nextAction))
            {
                player.NextAIAction = nextAction;
                if (nextAction != PlayerAction.NONE)
                    lastAction = nextAction;
            }
            else
                player.NextAIAction = PlayerAction.NONE;
        }
    }
}
