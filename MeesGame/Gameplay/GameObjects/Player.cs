using MeesGame.Gameplay.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class Player : SpriteGameObject
    {
        /// <summary>
        /// Contains all information about the player that may change during the game. Given to Tiles to edit when performing actions.
        /// </summary>
        private PlayerState state = new PlayerState();

        public Player(Level level, Point location, int layer = 0, string id = "", int score = 0) : base("player", layer, id)
        {
            Score = score;
            Level = level;
            Location = location;
            position = Level.Tiles.GetAnchorPosition(location);
        }

        /// <summary>
        /// Current score of the player
        /// </summary>
        public int Score
        {
            get
            {
                return state.Score;
            }

            private set
            {
                state.Score = value;
            }
        }

        /// <summary>
        /// The level that is played on
        /// </summary>
        public Level Level
        {
            get;
        }

        /// <summary>
        /// The last action performed by this player
        /// </summary>
        public PlayerAction LastAction
        {
            get; private set;
        }

        /// <summary>
        /// A list of all possible actions the player can take at the current stage of the game
        /// </summary>
        public ICollection<PlayerAction> Actions
        {
            get
            {
                List<PlayerAction> actions = new List<PlayerAction>();
                foreach (PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
                {
                    if (CanPerformAction(action)) actions.Add(action);
                }
                return actions;
            }
        }

        /// <summary>
        /// Checks if a player can perform the specified action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>true, if the player can perform the action. false otherwise.</returns>
        public virtual bool CanPerformAction(PlayerAction action)
        {

            if (CurrentTile.IsActionForbiddenFromHere(this, action)) return false;

            Point newLocation = CurrentTile.GetLocationAfterAction(action);
            return Level.Tiles.GetTile(newLocation).CanPlayerMoveHere(this);
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        public void PerformAction(PlayerAction action)
        {
            if (!CanPerformAction(action)) throw new PlayerActionNotAllowedException();
            LastAction = action;

            CurrentTile.PerformAction(this, state, action);
        }

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        public Tile CurrentTile
        {
            get
            {
                return Level.Tiles.GetTile(Location);
            }
        }

        /// <summary>
        /// The location of the player on the TileField
        /// </summary>
        public Point Location
        {
            get
            {
                return state.Location;
            }
            private set
            {
                state.Location = value;
            }
        }

        public bool HasKey()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// List of actions that can be used to move players
        /// </summary>
        public static readonly PlayerAction[] MOVEMENT_ACTIONS = new PlayerAction[] { NORTH, WEST, SOUTH, EAST };
    }

    /// <summary>
    /// An exception that is thrown when a Player tries to perform an Action that is not allowed.
    /// </summary>
    class PlayerActionNotAllowedException : Exception
    {

    }

    class TimedPlayer : Player
    {
        /// <summary>
        /// The time that the last action was performed
        /// </summary>
        protected TimeSpan lastActionTime;

        public TimedPlayer(Level level, Point location, int layer = 0, string id = "", int score = 0) : base(level, location, layer, id, score)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //if enough time has elapsed since the previous action, perform the selected action
            if (gameTime.TotalGameTime - lastActionTime >= Level.TimeBetweenActions)
            {
                if (CanPerformAction(NextAction) && NextAction != NONE)
                {
                    //important for smooth movement
                    velocity = CalculateVelocityVector(NextAction);
                    PerformAction(NextAction);
                    lastActionTime = gameTime.TotalGameTime;
                    NextAction = NONE;
                }
                else
                {
                    //stop moving
                    velocity = Vector2.Zero;

                    //put the player exactly at the middle of the square
                    position = Level.Tiles.GetAnchorPosition(Location);
                }
            }
            base.Update(gameTime);
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
        }
    }

    class HumanPlayer : TimedPlayer
    {
        public HumanPlayer(Level level, Point location, int layer = 0, string id = "", int score = 0) : base(level, location, layer, id, score)
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

    /// <summary>
    /// A player that can move over obstacles
    /// </summary>
    class EditorPlayer : HumanPlayer
    {
        public EditorPlayer(Level level, Point location, int layer = 0, string id = "", int score = 0) : base(level, location, layer, id, score)
        {
        }


        /// <summary>
        /// An editorplayer can move everywhere on the map. He only cannot move out of the maps bounds
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override bool CanPerformAction(PlayerAction action)
        {
            //An editorplayer can not do special moves
            if (action == PlayerAction.SPECIAL) return false;

            Point newLocation = CurrentTile.GetLocationAfterAction(action);
            //If the editorplayer may only not move out of the tilefield
            return !Level.Tiles.OutOfTileField(newLocation.X, newLocation.Y);
        }

    }

}
