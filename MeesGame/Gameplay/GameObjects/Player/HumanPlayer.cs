using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MeesGame.CharacterAction;

namespace MeesGame
{
    /// <summary>
    /// A timed player that performs actions based on input from the user.
    /// </summary>
    class HumanPlayer : Player
    {
        private CharacterAction nextAction = NONE;
        public HumanPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.W) || inputHelper.IsKeyDown(Keys.Up))
            {
                nextAction = NORTH;
            }
            else if (inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right))
            {
                nextAction = EAST;
            }
            else if (inputHelper.IsKeyDown(Keys.S) || inputHelper.IsKeyDown(Keys.Down))
            {
                nextAction = SOUTH;
            }
            else if (inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left))
            {
                nextAction = WEST;
            }
            else
            {
                nextAction = NONE;
            }
        }

        protected override CharacterAction NextAction
        {
            get
            {
                return nextAction;
            }
        }
    }
}
