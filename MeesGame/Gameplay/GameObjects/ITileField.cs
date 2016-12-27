using Microsoft.Xna.Framework;

namespace MeesGame
{
    interface ITileField
    {
        Tile GetTile(Point location);

        Tile GetTile(int x, int y);

        /// <summary>
        /// Returns the type of the tile at the given location
        /// </summary>
        TileType GetType(int x, int y);

        bool OutOfTileField(int x, int y);
    }
}