using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MeesGame
{
    interface IPlayer
    {
        /// <summary>
        /// All the PlayerActions the player can take at the currrent state of the game
        /// </summary>
        ICollection<PlayerAction> PossibleActions { get; }

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        Tile CurrentTile { get; }

        /// <summary>
        /// The last action the player performed
        /// </summary>
        PlayerAction LastAction { get; }

        /// <summary>
        /// The TileField that is being played on
        /// </summary>
        ITileField TileField { get; }

        /// <summary>
        /// The player's location on the TileField
        /// </summary>
        Point Location { get; }

        /// <summary>
        /// The player's score
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Check if the player has a key
        /// </summary>
        /// <returns></returns>
        bool HasKey();
    }
}
