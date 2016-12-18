using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.GameObjects
{
    class EndTile : FloorTile
    {
        GameStateManager gameStates;

        public EndTile(int layer = 0, string id = "") : base("end_door", TileType.Floor, layer, id)
        {

        }

        //Check if the player finished the level
        public void Finished(Player player)
        {
            if (player.Position == this.position)
                gameStates.SwitchTo("GameOverState");
            else
                return;
        }
    }
}
