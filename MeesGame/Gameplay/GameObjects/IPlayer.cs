using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MeesGame
{
    interface IPlayer
    {
        ICollection<PlayerAction> Actions { get; }
        Tile CurrentTile { get; }
        PlayerAction LastAction { get; }
        ITileField TileField { get; }
        Point Location { get; }
        int Score { get; }

        bool HasKey();
    }
}