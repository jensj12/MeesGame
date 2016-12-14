using static MeesGame.PlayerAction;

namespace MeesGame
{
    class DoorTile : WallTile
    {
        //TODO: Pair a door with a specific key.
        public DoorTile(int layer = 0, string id = "") : base("doorTile", TileType.Door, layer, id)
        {

        }

        //TODO implement doorIsOpen
        public override bool CanPlayerMoveHere(Player player)
        {
            //A player is allowed to move onto a door tile if the door is open.
            //if (DoorIsOpen(player))
            if (player.HasKey())
                return true;
            else
                return false;
        }

        public override bool IsActionForbiddenFromHere(Player player, PlayerAction action)
        {
            //A player can only move back to where he came from or into the opposite direction
            if (action == EAST || action == WEST)
            {
                if (player.LastAction == EAST || player.LastAction == WEST)
                    return false;
                else
                    return true;
            }
            else if (action == NORTH || action == SOUTH)
            {
                if (player.LastAction == NORTH || player.LastAction == SOUTH)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        /*TODO 
        public bool DoorIsOpen(Player player)
        {
            //A player is allowed to open a door if he has the right key.
            bool heeftSleutel = player.PlayerHasKey();
            return false;
        }*/

    }


    class KeyTile : FloorTile
    {

        public KeyTile(int layer = 0, string id = "") : base("keyTile", TileType.Key, layer, id)
        {

        }

        //TODO: if key has been picked up, keytile changes into floortile.
    }
}
