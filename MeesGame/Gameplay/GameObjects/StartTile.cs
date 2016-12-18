using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.GameObjects
{
    class StartTile : FloorTile
    {
        Level level;
        TileField tileField;

        public StartTile(int layer = 0, string id = "") : base("floorTile", TileType.Floor, layer, id)  //Change the name of the sprite once there is one
        {
            this.position = tileField.GetAnchorPosition(level.Start);
        }
    }
}
