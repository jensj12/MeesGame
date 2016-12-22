using Microsoft.Xna.Framework;
using System;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class TimedPlayer : Player, IPlayer
    {
        /// <summary>
        /// The time that the last action was performed
        /// </summary>
        protected TimeSpan lastActionTime;

        protected TimeSpan lastAnimationTime;

        public TimedPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animate(gameTime);
            if (!IsMoving)
            {
                if (CanPerformAction(NextAction) && NextAction != NONE)
                {
                    PerformAction(NextAction);



                    lastActionTime = gameTime.TotalGameTime;
                    NextAction = NONE;
                }
                else
                {
                    this.Sprite.SheetIndex = 0;
                }
            }
        }

        /// <summary>
        /// Calculates a Vector for the animation velocity of the player so that the player removes smoothly between tiles
        /// </summary>
        /// <param name="action">The action that should be animated</param>
        /// <returns></returns>
        private Vector2 CalculateVelocityVector(PlayerAction action)
        {
            Vector2 direction = GetDirectionVector(action);
            float speed = Speed;
            return new Vector2(direction.X * speed, direction.Y * speed);
        }

        /// <summary>
        /// Gets a direction vector based on the action of the player
        /// </summary>
        /// <param name="action">The action that the direction vector should represent</param>
        /// <returns>A Vector with absolute value of 1 in the direction of movement associated with the action</returns>
        private Vector2 GetDirectionVector(PlayerAction action)
        {
            switch (action)
            {
                case NORTH:
                    return new Vector2(0, -1);

                case EAST:
                    return new Vector2(1, 0);

                case SOUTH:
                    return new Vector2(0, 1);

                case WEST:
                    return new Vector2(-1, 0);
            }
            return new Vector2(0, 0);
        }

        public void animate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastAnimationTime.TotalMilliseconds >= this.Level.TimeBetweenActions.TotalMilliseconds / 4)
            {
                this.Sprite.SheetIndex = (this.Sprite.SheetIndex + 1) % 4;
                lastAnimationTime = gameTime.TotalGameTime;
            }
        }

        /// <summary>
        /// The speed of the player for animation in px per seconds
        /// </summary>
        private float Speed
        {
            get
            {
                return (float)(Level.Tiles.CellHeight / Level.TimeBetweenActions.TotalSeconds);
            }
        }

        /// <summary>
        /// The action that will be performed as soon as another action can be performed
        /// </summary>
        public PlayerAction NextAction
        {
            get; set;
        } = NONE;

        public ITileField TileField
        {
            get
            {
                return Level.Tiles;
            }
        }
    }
}
