using System.Collections.Generic;
using Microsoft.Xna.Framework;

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