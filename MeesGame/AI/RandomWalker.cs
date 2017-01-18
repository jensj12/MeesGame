using MeesGame;

namespace AI
{
    class RandomWalker : IAI
    {
        IAIPlayer player;

        public void GameStart(IAIPlayer player, int difficulty)
        {
            this.player = player;
        }

        public PlayerAction GetNextAction()
        {
            IPlayer dummy = player.DummyPlayer;
            return dummy.PossibleActions[GameEnvironment.Random.Next(dummy.PossibleActions.Count)];
        }
    }
}
