using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static MeesGame.CharacterAction;

namespace MeesGame
{
    class Character : ITileFieldPlayer, ICharacter
    {
        /// <summary>
        /// List of actions that can be used to move characters
        /// </summary>
        public static readonly CharacterAction[] MOVEMENT_ACTIONS = new CharacterAction[] { NORTH, WEST, SOUTH, EAST };

        public delegate void CharacterEventHandler(Character character);
        public delegate void CharacterDirectionEventHandler(Character character, Direction direction);

        /// <summary>
        /// Event called when the character moves smoothly in a direction
        /// </summary>
        public event CharacterDirectionEventHandler OnMoveSmoothly;

        /// <summary>
        /// Event called when the character has won the game
        /// </summary>
        public event CharacterEventHandler OnCharacterWin;

        /// <summary>
        /// Event called when the character has lost the game
        /// </summary>
        public event CharacterEventHandler OnCharacterLose;

        /// <summary>
        /// Event called when a character performs an action
        /// </summary>
        public event CharacterEventHandler OnCharacterAction;

        /// <summary>
        /// Creates a character that plays on a TileField
        /// </summary>
        /// <param name="tileField">The tilefield</param>
        /// <param name="location">The initial location on the tilefield</param>
        /// <param name="score">The initial score. Default is 0.</param>
        public Character(TileField tileField, Point location, int score = 0)
        {
            TileField = tileField;
            this.location = location;
            Score = score;
            Inventory = new Inventory();
        }

        /// <summary>
        /// Makes the character win the game
        /// </summary>
        public void Win()
        {
            OnCharacterWin?.Invoke(this);
        }

        /// <summary>
        /// Makes the character lose the game
        /// </summary>
        public void Lose()
        {
            OnCharacterLose?.Invoke(this);
        }

        /// <summary>
        /// A list of all possible actions the character can take in the current state of the game
        /// </summary>
        public IList<CharacterAction> PossibleActions
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
        /// Checks if a character can perform the specified action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>true, if the character can perform the action. false otherwise.</returns>
        public virtual bool CanPerformAction(CharacterAction action)
        {
            return !CurrentTile.IsActionForbiddenFromHere(this, action);
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        public void PerformAction(CharacterAction action)
        {
            if (!CanPerformAction(action)) throw new PlayerActionNotAllowedException();

            CurrentTile.PerformAction(this, action);

            OnCharacterAction?.Invoke(this);
        }

        /// <summary>
        /// Current score of the character
        /// </summary>
        public int Score
        {
            get; set;
        }

        /// <summary>
        /// The last action performed by this character
        /// </summary>
        public CharacterAction LastAction
        {
            get; set;
        }

        /// <summary>
        /// The Tile the character is currently on
        /// </summary>
        public Tile CurrentTile
        {
            get
            {
                return TileField.GetTile(Location);
            }
        }

        /// <summary>
        /// The inventory of the character
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
        /// The location of the character on the TileField
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
        /// Makes the character move in the specified direction. Calls the OnMoveSmoothly event
        /// </summary>
        /// <param name="direction">The direction the character should move to.</param>
        public void MoveSmoothly(Direction direction)
        {
            OnMoveSmoothly?.Invoke(this, direction);
            Location += direction.ToPoint();
        }

        /// <summary>
        /// Call this method when the character has fully moved to his new location
        /// </summary>
        public void EndMoveSmoothly()
        {
            CurrentTile.EnterCenterOfTile(this);
        }

        /// <summary>
        /// Creates an untimed clone of this character
        /// </summary>
        /// <returns></returns>
        public ICharacter Clone()
        {
            Character clone = new UntimedCharacter(TileField, Location, Score);
            foreach (InventoryItem item in Inventory.Items)
            {
                clone.Inventory.Items.Add(item);
            }
            return clone;
        }

        /// <summary>
        /// Checks if the character has an item of a certain type
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public bool HasItem(InventoryItemType itemType)
        {
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == itemType)
                    return true;
            return false;
        }
    }
}
