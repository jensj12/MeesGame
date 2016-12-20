using Microsoft.Xna.Framework;

namespace MeesGame
{
    class StartTile : FloorTile
    {
        public StartTile(int layer = 0, string id = "") : base("floorTile", TileType.Start, layer, id)  //Change the name of the sprite once there is one
        {

        }
    }
}
