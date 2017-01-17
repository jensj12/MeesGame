using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MeesGame
{
    public interface ICharacter
    {
        /// <summary>
        /// A list of all possible actions the player can take in the current state of the game
        /// </summary>
        IList<CharacterAction> PossibleActions { get; }

        /// <summary>
        /// The TileField the player is playing on
        /// </summary>
        TileField TileField { get; }

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        Tile CurrentTile { get; }

        /// <summary>
        /// The last action the player performed
        /// </summary>
        CharacterAction LastAction { get; }

        /// <summary>
        /// Make the player perform a specified action
        /// </summary>
        void PerformAction(CharacterAction action);

        /// <summary>
        /// The player's location on the TileField
        /// </summary>
        Point Location { get; }

        /// <summary>
        /// The items of the player
        /// </summary>
        Inventory Inventory { get; }

        /// <summary>
        /// The player's score
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Check if the player has a key
        /// </summary>
        /// <returns></returns>
        bool HasItem(InventoryItemType itemType);

        /// <summary>
        /// Creates of copy of the player at its current position
        /// </summary>
        /// <returns></returns>
        ICharacter Clone();
    }
}
