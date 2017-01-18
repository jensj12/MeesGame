using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    public interface ITileFieldPlayer
    {
        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        Tile CurrentTile { get; }

        /// <summary>
        /// The inventory of the player
        /// </summary>
        Inventory Inventory { get; }

        /// <summary>
        /// The last action performed by this player
        /// </summary>
        PlayerAction LastAction { get; set; }

        /// <summary>
        /// The location of the player on the TileField
        /// </summary>
        Point Location { get; }

        /// <summary>
        /// A list of all possible actions the player can take in the current state of the game
        /// </summary>
        IList<PlayerAction> PossibleActions { get; }

        /// <summary>
        /// Current score of the player
        /// </summary>
        int Score { get; set; }
        TileField TileField { get; }

        /// <summary>
        /// Checks if a player can perform the specified action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>true, if the player can perform the action. false otherwise.</returns>
        bool CanPerformAction(PlayerAction action);

        /// <summary>
        /// Checks if the player has a key
        /// </summary>
        /// <returns>Whether the player has the key</returns>
        bool HasItem(InventoryItemType itemType);

        /// <summary>
        /// Makes the player lose the game
        /// </summary>
        void Lose();

        /// <summary>
        /// Makes the player move in the specified direction. Calls the OnMoveSmoothly event
        /// </summary>
        /// <param name="direction">The direction the player should move to.</param>
        void MoveSmoothly(Direction direction);

        /// <summary>
        /// Call this method when the player has fully moved to his new location
        /// </summary>
        void EndMoveSmoothly();

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        void PerformAction(PlayerAction action);

        /// <summary>
        /// Makes the player win the game
        /// </summary>
        void Win();
    }
}
