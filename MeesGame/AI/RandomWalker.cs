using MeesGame;
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

            // Pick a random action out of the possible action
            nextAction = dummy.PossibleActions[GameEnvironment.Random.Next(dummy.PossibleActions.Count)];
        }

        public void UpdateNextAction()
        {
            if (player.DummyPlayer.CanPerformAction(nextAction))
                player.NextAIAction = nextAction;
            else
                player.NextAIAction = PlayerAction.NONE;
        }
    }
}
