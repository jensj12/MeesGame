using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace MeesGame
{
    /// <summary>
    /// Smoothly moving, animated game object that is affected by the tiles on the tilefield.
    /// Can perform PlayerActions on the TileField affecting himself. After a while he might either
    /// win or lose.
    /// Has a score and an inventory.
    /// </summary>
    public class PlayerGameObject : AnimatedMovingGameObject
    {
        public delegate void PlayerEventHandler(PlayerGameObject player);

        /// <summary>
        /// Event called when the player has won the game
        /// </summary>
        public event PlayerEventHandler OnPlayerWin;

        /// <summary>
        /// Event called when the player has lost the game
        /// </summary>
        public event PlayerEventHandler OnPlayerLose;

        /// <summary>
        /// Event called when a player performs an action
        /// </summary>
        public event PlayerEventHandler OnPlayerAction;

        /// Used for starting and stopping the (walking) sound
        SoundEffectInstance soundFootsteps;

        /// <summary>
        /// The DummyPlayer that this object represents
        /// </summary>
        DummyPlayer player;

        /// <summary>
        /// Creates a new player for a specific level
        /// </summary>
        /// <param name="level">The Level that the player should play in</param>
        /// <param name="location">The starting location of the player</param>
        /// <param name="score">The initial score of the player. Default is 0.</param>
        public PlayerGameObject(Level level, Point location, int score = 0) : base(level.Tiles, level.TimeBetweenActions, "player@4x4", 0, "")
        {
            player = new DummyPlayer(level.Tiles, location, score);
            Level = level;
            Teleport(location);
            TileField.RevealArea(location);
            soundFootsteps = GameEnvironment.AssetManager.Content.Load<SoundEffect>("footsteps").CreateInstance();

            player.OnPlayerAction += delegate (DummyPlayer player) { OnPlayerAction?.Invoke(this); };
            player.OnPlayerWin += delegate (DummyPlayer player) { OnPlayerWin?.Invoke(this); };
            player.OnPlayerLose += delegate (DummyPlayer player)
            {
                OnPlayerLose?.Invoke(this);
                GameEnvironment.AssetManager.PlaySound("scream");
            };
            player.OnMoveSmoothly += delegate (DummyPlayer player, Direction direction)
            {
                MoveSmoothly(direction);
                soundFootsteps.Play();
            };
            player.OnTeleport += delegate (DummyPlayer player) 
            {
                Teleport(player.Location);
                GameEnvironment.AssetManager.PlaySound("climbing_sound");
            };
            StoppedMoving += delegate (GameObject obj)
            {
                player.EndMoveSmoothly();
                soundFootsteps.Stop();
            };
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
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        protected void PerformAction(PlayerAction action)
        {
            if (!player.CanPerformAction(action)) throw new PlayerActionNotAllowedException();

            LastAction = action;
            if (action.IsDirection()) Direction = action.ToDirection();

            player.PerformAction(action);

            TileField.Visit(Location);
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
        /// The inventory of the player
        /// </summary>
        public Inventory Inventory
        {
            get
            {
                return player.Inventory;
            }
        }

        public TileField TileField
        {
            get
            {
                return Level.Tiles;
            }
        }

        /// <summary>
        /// A DummyPlayer that is in the same state as this one.
        /// </summary>
        public IPlayer DummyPlayer
        {
            get
            {
                return player.Clone();
            }
        }

        protected virtual PlayerAction NextAction
        {
            get
            {
                return PlayerAction.NONE;
            }
        }

        protected override bool IsSliding
        {
            get
            {
                if ((TileField.GetTile(Location) is IceTile) || TileField.GetTile(Location - Direction.ToPoint()) is IceTile)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            PlayerAction nextAction = NextAction;

            if (!IsMoving && player.CanPerformAction(nextAction))
            {
                PerformAction(nextAction);
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
