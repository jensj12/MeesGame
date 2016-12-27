﻿using Microsoft.Xna.Framework;
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
        /// Event called when a player performs an action
        /// </summary>
        public event PlayerEventHandler PlayerAction;

        /// <summary>
        /// Creates a new player for a specific level
        /// </summary>
        /// <param name="level">The Level that the player should play in</param>
        /// <param name="location">The starting location of the player</param>
        /// <param name="score">The initial score of the player. Default is 0.</param>
        public Player(Level level, Point location, int score = 0) : base(level.Tiles, level.TimeBetweenActions, "playerdown@4", 0, "")
        {
            Score = score;
            Level = level;
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
        public ICollection<PlayerAction> PossibleActions
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
                sprite = new SpriteSheet(getAssetNameFromDirection(action.ToDirection()));
            }

            CurrentTile.PerformAction(this, action);
            InventoryItem item = CurrentTile.GetItem();
            if (item != null)
                Inventory.Items.Add(item);

            this.Level.Tiles.RevealArea(Location);
            CurrentTile.IsVisited = true;

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
        public bool HasKey()
        {
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == InventoryItemType.Key)
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

        // TODO: make the player lose instead of win
        public void Lose()
        {
            PlayerWin?.Invoke(this);
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

        private string getAssetNameFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return "playerup@4";
                case Direction.EAST:
                    return "playerright@4";
                case Direction.SOUTH:
                    return "playerdown@4";
                case Direction.WEST:
                    return "playerleft@4";
            }
            throw new IndexOutOfRangeException();
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
