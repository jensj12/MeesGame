using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class Player : SpriteGameObject
    {
        protected Level level;
        protected int score = 0;
        protected Point location;
        protected TimeSpan lastActionTime;
        protected PlayerAction nextAction = NONE;

        public Player(Level level, TileField tileField, Point location, int layer = 0, string id = "", int score = 0) : base("player", layer, id)
        {
            this.score = score;
            this.level = level;
            this.location = location;
            this.position = tileField.GetAnchorPosition(location);
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public Level Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        //this is just a simple implementation
        //an action is allowed if and only if it is a direction
        //we might change this later

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
        public bool CanPerformAction(PlayerAction action)
        {
            //for now, special actions are never allowed
            if (action == SPECIAL) return false;

            //movements are allowed as long as the new tile is of type floor
            Point newLocation = GetLocationAfterAction(action);
            return level.Tiles.GetType(newLocation) == TileType.Floor;
        }

        public Point GetLocationAfterAction(PlayerAction action)
        {
            Point newLocation = this.location;
            switch (action)
            {
                case NORTH:
                    newLocation.Y--;
                    break;
                case EAST:
                    newLocation.X++;
                    break;
                case SOUTH:
                    newLocation.Y++;
                    break;
                case WEST:
                    newLocation.X--;
                    break;
            }
            return newLocation;
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        public void PerformAction(PlayerAction action)
        {
            if (!CanPerformAction(action)) throw new PlayerActionNotAllowedException();

            this.location = GetLocationAfterAction(action);
        }

        public override void Update(GameTime gameTime)
        {
            //if enough time has elapsed since the previous action, perform the selected action
            if (gameTime.TotalGameTime - lastActionTime >= level.TimeBetweenActions && CanPerformAction(nextAction))
            {
                PerformAction(nextAction);
                lastActionTime = gameTime.TotalGameTime;
                nextAction = NONE;
            }

            //no animation yet
            position = level.Tiles.GetAnchorPosition(location);
            base.Update(gameTime);
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
        }
    }

    /// <summary>
    /// An exception that is thrown when a Player tries to perform an Action that is not allowed.
    /// </summary>
    class PlayerActionNotAllowedException : Exception
    {

    }
}
