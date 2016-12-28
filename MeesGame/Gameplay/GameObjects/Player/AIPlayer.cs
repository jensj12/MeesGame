using AI;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class AIPlayer : TimedPlayer
    {
        IAI AI;

        public AIPlayer(IAI AI, Level level, Point location, int score = 0):base(level, location, score)
        {
            this.AI = AI;
            AI.GameStart(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (NextAction == MeesGame.PlayerAction.NONE && !IsMoving)
            {
                NextAction = AI.GetNextAction();
            }
            base.Update(gameTime);
        }
    }
}
