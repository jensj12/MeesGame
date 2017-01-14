using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    /// <summary>
    /// Smoothly moving, animated game object that is affected by the tiles on the tilefield.
    /// Can perform PlayerActions on the TileField affecting himself. After a while he might either
    /// win or lose.
    /// Has a score and an inventory.
    /// </summary>
    public class Player : AnimatedMovingGameObject, IPlayer
    {
        public delegate void PlayerEventHandler(Player player);

        /// <summary>
        /// Event called when the player has won the game
        /// </summary>
        public event PlayerEventHandler PlayerWin;

        /// <summary>
        /// Event called when the player has lost the game
        /// </summary>
        public event PlayerEventHandler PlayerLose;

        /// <summary>
        /// Event called when a player performs an action
        /// </summary>
        public event PlayerEventHandler PlayerAction;

        //whether a player can reveal an area or not
        private bool canRevealArea;

        /// <summary>
        /// Creates a new player for a specific level
        /// </summary>
        /// <param name="level">The Level that the player should play in</param>
        /// <param name="location">The starting location of the player</param>
        /// <param name="score">The initial score of the player. Default is 0.</param>
        public Player(Level level, Point location, int score = 0, bool canRevealArea = true) : base(level.Tiles, level.TimeBetweenActions, "player@4x4", 0, "")
        {
            Score = score;
            Level = level;
            this.canRevealArea = canRevealArea;
            Teleport(location);
            Inventory = new Inventory();
            Level.Tiles.RevealArea(location);
            LocationChanged += delegate (GameObject obj) { CurrentTile.EnterTile(this); };
            StoppedMoving += delegate (GameObject obj) { CurrentTile.EnterCenterOfTile(this); };
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

            if (action.IsDirection())
            {
                switch (action)
                {
                    case NORTH:
                        Sprite.SpriteRowIndex = 0;
                        break;
                    case EAST:
                        Sprite.SpriteRowIndex = 1;
                        break;
                    case SOUTH:
                        Sprite.SpriteRowIndex = 2;
                        break;
                    case WEST:
                        Sprite.SpriteRowIndex = 3;
                        break;
                }
            }

            CurrentTile.PerformAction(this, action);

            if(canRevealArea)
            {
                Level.Tiles.RevealArea(Location);
            }

            PlayerAction?.Invoke(this);
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
        /// Checks if the player has a key
        /// </summary>
        /// <returns></returns>
        public bool HasItem(InventoryItemType itemType)
        {
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == itemType)
                    return true;
            return false;
        }

        /// <summary>
        /// Makes the player win the game
        /// </summary>
        public void Win()
        {
            PlayerWin?.Invoke(this);
        }

        /// <summary>
        /// Makes the player lose the game
        /// </summary>
        public void Lose()
        {
            PlayerLose?.Invoke(this);
        }

        /// <summary>
        /// List of actions that can be used to move players
        /// </summary>
        public static readonly PlayerAction[] MOVEMENT_ACTIONS = new PlayerAction[] { NORTH, WEST, SOUTH, EAST };

        /// <summary>
        /// The inventory of the player
        /// </summary>
        public Inventory Inventory
        {
            get;
        }

        /// <summary>
        /// Creates of copy of the player at its current position
        /// </summary>
        /// <returns></returns>
        IPlayer IPlayer.Clone()
        {
            IPlayer newPlayer = new Player(Level, Location, Score);
            foreach (InventoryItem item in Inventory.Items)
            {
                newPlayer.Inventory.Items.Add(item);
            }
            return newPlayer;
        }
    }

    /// <summary>
    /// An exception that is thrown when a Player tries to perform an Action that is not allowed.
    /// </summary>
    class PlayerActionNotAllowedException : Exception
    {
    }
}
