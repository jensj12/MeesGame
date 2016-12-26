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

                    if (NextAction == NORTH)
                    {
                        this.sprite = new SpriteSheet("playerup@4");
                    }
                    else if (NextAction == SOUTH)
                    {
                        this.sprite = new SpriteSheet("playerdown@4");
                    }
                    else if (NextAction == EAST)
                    {
                        this.sprite = new SpriteSheet("playerright@4");
                    }
                    else if (NextAction == WEST)
                    {
                        this.sprite = new SpriteSheet("playerleft@4");
                    }

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

        public ITileField TileField
        {
            get
            {
                return Level.Tiles;
            }
        }
    }
}
