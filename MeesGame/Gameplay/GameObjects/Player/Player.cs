using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace MeesGame
{
    /// <summary>
    /// Smoothly moving, animated game object that is affected by the tiles on the tilefield.
    /// Visual timed representation of a Character
    /// </summary>
    public class Player : AnimatedMovingGameObject
    {
        public delegate void PlayerEventHandler(Player player);

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
        /// The Character that this player represents
        /// </summary>
        Character character;

        /// <summary>
        /// Creates a new player for a specific level
        /// </summary>
        /// <param name="level">The Level that the player should play in</param>
        /// <param name="location">The starting location of the player</param>
        /// <param name="score">The initial score of the player. Default is 0.</param>
        public Player(Level level, Point location, int score = 0) : base(level.Tiles, level.TimeBetweenActions, "player@4x4", 0, "")
        {
            character = new Character(level.Tiles, location, score);
            Level = level;
            Teleport(location);
            TileField.RevealArea(location);
            soundFootsteps = GameEnvironment.AssetManager.Content.Load<SoundEffect>("footsteps").CreateInstance();

            character.OnCharacterAction += delegate (Character player) { OnPlayerAction?.Invoke(this); };
            character.OnCharacterWin += delegate (Character player) { OnPlayerWin?.Invoke(this); };
            character.OnCharacterLose += delegate (Character player) { OnPlayerLose?.Invoke(this); };
            character.OnMoveSmoothly += delegate (Character player, Direction direction) {
                MoveSmoothly(direction);
                soundFootsteps.Play();
            };
            StoppedMoving += delegate (GameObject obj) {
                character.EndMoveSmoothly();
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
        public CharacterAction LastAction
        {
            get; private set;
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        protected void PerformAction(CharacterAction action)
        {
            if (!character.CanPerformAction(action)) throw new PlayerActionNotAllowedException();

            LastAction = action;
            if (action.IsDirection()) Direction = action.ToDirection();

            character.PerformAction(action);

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
                return character.Inventory;
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
        public ICharacter Character
        {
            get
            {
                return character.Clone();
            }
        }

        protected virtual CharacterAction NextAction
        {
            get
            {
                return CharacterAction.NONE;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CharacterAction nextAction = NextAction;

            if (!IsMoving && character.CanPerformAction(nextAction))
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
