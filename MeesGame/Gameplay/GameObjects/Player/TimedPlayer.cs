﻿using Microsoft.Xna.Framework;
using System;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    /// <summary>
    /// A player that needs to wait for his actions to complete before he can perform the next action.
    /// </summary>
    class TimedPlayer : Player, IPlayer
    {
        /// <summary>
        /// The time that the last action was performed
        /// </summary>
        protected TimeSpan lastActionTime;

        public TimedPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (!IsMoving)
            {
                if (CanPerformAction(NextAction) && NextAction != NONE)
                {
                    PerformAction(NextAction);

                    lastActionTime = gameTime.TotalGameTime;
                    NextAction = NONE;
                }
            }
        }
        
        /// <summary>
        /// The action that will be performed as soon as another action can be performed
        /// </summary>
        public PlayerAction NextAction
        {
            get; set;
        } = NONE;

        /// <summary>
        /// The TileField that the player is on
        /// </summary>
        public ITileField TileField
        {
            get
            {
                return Level.Tiles;
            }
        }
    }
}
