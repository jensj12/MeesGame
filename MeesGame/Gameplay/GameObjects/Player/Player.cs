using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    public class Player : SmoothlyMovingGameObject
    {
        public delegate void PlayerActionHandler(PlayerAction action);
        public delegate void PlayerWinHandler(Player player);
        public event PlayerWinHandler PlayerWin;
        public event PlayerActionHandler PlayerAction;

        public Player(Level level, Point location, int layer = 0, string id = "", int score = 0) : base(level.Tiles,level.TimeBetweenActions,"playerdown@4", layer, id)
        {
            Score = score;
            Level = level;
            Teleport(location);
            Inventory = new Inventory();
            Level.Tiles.RevealArea(location);
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

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();
            CurrentTile.EnterTile(this);
        }

        protected override void StopMoving()
        {
            base.StopMoving();
            CurrentTile.EnterCenterOfTile(this);
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
                Inventory.Items.Add(item);

            this.Level.Tiles.RevealArea(Location);
            CurrentTile.IsVisited = true;

            PlayerActionEvent(action);
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
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == InventoryItemType.Key)
                    return true;
            return false;
        }

        public void Win()
        {
            PlayerWin?.Invoke(this);
        }

        // TODO: make the player lose instead of win
        public void Lose()
        {
            PlayerWin?.Invoke(this);
        }

        /// <summary>
        /// List of actions that can be used to move players
        /// </summary>
        public static readonly PlayerAction[] MOVEMENT_ACTIONS = new PlayerAction[] { NORTH, WEST, SOUTH, EAST };

        public void PlayerActionEvent(PlayerAction action)
        {
            PlayerAction?.Invoke(action);
        }

        public Inventory Inventory
        {
            get;
        }
    }

    /// <summary>
    /// An exception that is thrown when a Player tries to perform an Action that is not allowed.
    /// </summary>
    class PlayerActionNotAllowedException : Exception
    {
    }
}
