using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class DummyPlayer : ITileFieldPlayer, IPlayer
    {
        /// <summary>
        /// List of actions that can be used to move players
        /// </summary>
        public static readonly PlayerAction[] MOVEMENT_ACTIONS = new PlayerAction[] { NORTH, WEST, SOUTH, EAST };

        public delegate void DummyPlayerEventHandler(DummyPlayer player);
        public delegate void DummyPlayerDirectionEventHandler(DummyPlayer player, Direction direction);

        /// <summary>
        /// Event called when the player moves smoothly in a direction
        /// </summary>
        public event DummyPlayerDirectionEventHandler OnMoveSmoothly;

        /// <summary>
        /// Event called when the player has won the game
        /// </summary>
        public event DummyPlayerEventHandler OnPlayerWin;

        /// <summary>
        /// Event called when the player has lost the game
        /// </summary>
        public event DummyPlayerEventHandler OnPlayerLose;

        /// <summary>
        /// Event called when a player performs an action
        /// </summary>
        public event DummyPlayerEventHandler OnPlayerAction;

        public DummyPlayer(TileField tileField, Point location, int score = 0)
        {
            TileField = tileField;
            this.location = location;
            Score = score;
            Inventory = new Inventory();
        }

        /// <summary>
        /// Makes the player win the game
        /// </summary>
        public void Win()
        {
            OnPlayerWin?.Invoke(this);
        }

        /// <summary>
        /// Makes the player lose the game
        /// </summary>
        public void Lose()
        {
            OnPlayerLose?.Invoke(this);
        }

        /// <summary>
        /// A list of all possible actions the player can take in the current state of the game
        /// </summary>
        public IList<PlayerAction> PossibleActions
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
            return !CurrentTile.IsActionForbiddenFromHere(this, action);
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        public void PerformAction(PlayerAction action)
        {
            if (!CanPerformAction(action)) throw new PlayerActionNotAllowedException();

            CurrentTile.PerformAction(this, action);

            OnPlayerAction?.Invoke(this);
        }

        /// <summary>
        /// Current score of the player
        /// </summary>
        public int Score
        {
            get; set;
        }

        /// <summary>
        /// The last action performed by this player
        /// </summary>
        public PlayerAction LastAction
        {
            get; set;
        }

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        public Tile CurrentTile
        {
            get
            {
                return TileField.GetTile(Location);
            }
        }

        /// <summary>
        /// The inventory of the player
        /// </summary>
        public Inventory Inventory
        {
            get;
        }

        /// <summary>
        /// The TileField that this is playing on
        /// </summary>
        public TileField TileField
        {
            get;
        }

        private Point location;
        /// <summary>
        /// The location of the player on the TileField
        /// </summary>
        public Point Location
        {
            get
            {
                return location;
            }
            private set
            {
                location = value;
                CurrentTile.EnterTile(this);
            }
        }

        /// <summary>
        /// Makes the player move in the specified direction. Calls the OnMoveSmoothly event
        /// </summary>
        /// <param name="direction">The direction the player should move to.</param>
        public void MoveSmoothly(Direction direction)
        {
            OnMoveSmoothly?.Invoke(this, direction);
            Location += direction.ToPoint();
        }

        /// <summary>
        /// Call this method when the player has fully moved to his new location
        /// </summary>
        public void EndMoveSmoothly()
        {
            CurrentTile.EnterCenterOfTile(this);
        }

        public IPlayer Clone()
        {
            DummyPlayer clone = new UntimedPlayer(TileField, Location, Score);
            foreach (InventoryItem item in Inventory.Items)
            {
                clone.Inventory.Items.Add(item);
            }
            return clone;
        }

        public bool HasItem(InventoryItemType itemType)
        {
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == itemType)
                    return true;
            return false;
        }
    }
}
