using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class Player : SmoothlyMovingGameObject
    {
        public Player(Level level, Point location, int score = 0) : base(level.Tiles, level.TimeBetweenActions, "player")
        {
            Score = score;
            Level = level;
            Teleport(location);
        }

        /// <summary>
        /// Current score of the player
        /// </summary>
        public int Score
        {
            get; set;
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

            CurrentTile.PerformAction(this, action);
            InventoryItem item = CurrentTile.GetItem();
            if (item != null)
                state.Inventory.Items.Add(item)
        }

        public Inventory Inventory
        {
            get;
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

        public bool HasKey()
        {
            foreach (InventoryItem item in (state.Inventory.Items))
                if (item.type == InventoryItemType.Key)
                    return true;
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
    }

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
