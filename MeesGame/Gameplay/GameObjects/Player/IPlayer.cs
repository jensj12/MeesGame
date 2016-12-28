﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MeesGame
{
    interface IPlayer
    {
        /// <summary>
        /// A list of all possible actions the player can take in the current state of the game
        /// </summary>
        IList<PlayerAction> PossibleActions { get; }

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        Tile CurrentTile { get; }

        /// <summary>
        /// The last action the player performed
        /// </summary>
        PlayerAction LastAction { get; }

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
        bool HasKey();

        /// <summary>
        /// Creates of copy of the player at its current position
        /// </summary>
        /// <returns></returns>
        IPlayer Clone();
    }
}