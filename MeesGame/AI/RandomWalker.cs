using MeesGame;

namespace AI
{
    class RandomWalker : IAI
    {
        IPlayer player;

        public void GameStart(IPlayer player, int difficulty)
        {
            this.player = player;
        }

        public PlayerAction GetNextAction()
        {
            return player.PossibleActions[GameEnvironment.Random.Next(player.PossibleActions.Count)];
        }
    }
}
