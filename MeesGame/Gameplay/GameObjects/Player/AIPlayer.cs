using AI;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class AIPlayer : PlayerGameObject, IAIPlayer
    {
        IAI AI;

        public AIPlayer(IAI AI, Level level, Point location, int score = 0) : base(level, location, score)
        {
            this.AI = AI;
            AI.GameStart(this, 3);
        }

        protected override PlayerAction NextAction
        {
            get
            {
                return AI.GetNextAction();
            }
        }
    }
}
