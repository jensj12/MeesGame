﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    /// <summary>
    /// A timed player that performs actions based on input from the user.
    /// </summary>
    class HumanPlayer : TimedPlayer
    {
        public HumanPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.W) || inputHelper.IsKeyDown(Keys.Up))
            {
                NextAction = NORTH;
            }
            else if (inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right))
            {
                NextAction = EAST;
            }
            else if (inputHelper.IsKeyDown(Keys.S) || inputHelper.IsKeyDown(Keys.Down))
            {
                NextAction = SOUTH;
            }
            else if (inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left))
            {
                NextAction = WEST;
            }
            else
            {
                NextAction = NONE;
            }
        }
    }
}