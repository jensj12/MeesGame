using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MeesGame.CharacterAction;

namespace MeesGame.Gameplay.GameObjects
{
    class Character : SpriteGameObject
    {
        protected Level level;
        protected int score = 0;
        protected Point location;
        protected TimeSpan lastActionTime;
        protected CharacterAction nextAction = NONE;

        //base met player omdat de psrite nog player heet en Content.mgcb doet raar
        public Character(Level level, TileField tileField, Point location, int layer = 0, string id = "", int score = 0) : base("player", layer, id)
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
        public ICollection<CharacterAction> Actions
        {
            get
            {
                List<CharacterAction> actions = new List<CharacterAction>();
                foreach (CharacterAction action in Enum.GetValues(typeof(CharacterAction)))
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
        public bool CanPerformAction(CharacterAction action)
        {
            //for now, special actions are never allowed
            if (action == SPECIAL) return false;

            //movements are allowed as long as the new tile is of type floor
            Point newLocation = GetLocationAfterAction(action);
            return level.Tiles.GetType(newLocation) == TileType.Floor;
        }

        public Point GetLocationAfterAction(CharacterAction action)
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
        public void PerformAction(CharacterAction action)
        {
            if (!CanPerformAction(action)) throw new ActionNotAllowedException();

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
    }
}
