using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.GameObjects.Tiles
{
    class UnknownTile : Tile
    {
        public override string[] textureNames
        {
            get
            {
                return new string[]
                {
                    "fourWayWallTile"
                };
            }
        }
    }
}
