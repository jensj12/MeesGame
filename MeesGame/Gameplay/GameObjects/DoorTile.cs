using Microsoft.Xna.Framework;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class DoorTile : WallTile
    {
        protected bool doorIsOpen = false;

        //TODO: Pair a door with a specific key.
        public DoorTile(int layer = 0, string id = "") : base("horizontalDoor", TileType.Door, layer, id)
        {
            secondarySprite = new SpriteSheet("horizontalDoorOverlay");
            secondarySpriteColor = Color.Blue;
        }

        public override bool CanPlayerMoveHere(Player player)
        {
            //Temporary OpenDoor call.
            OpenDoor(player);
            //A player is allowed to move onto a door tile if the door is open.
            if (doorIsOpen)
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

        public void OpenDoor(Player player)
        {
            //A player is only allowed to open a door if he has the right key.
            if (player.HasKey())
            {
                doorIsOpen = true;
            }
        }
        /*/ TODO 
        public override void UpdateGraphicsToMatchSurroundings()
        {
            int sheetIndex = 0;
            int x = Location.X;
            int y = Location.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile) sheetIndex += 1;
            if (tileField.GetTile(x + 1, y) is WallTile) sheetIndex += 2;
            if (tileField.GetTile(x, y + 1) is WallTile) sheetIndex += 4;
            if (tileField.GetTile(x - 1, y) is WallTile) sheetIndex += 8;
            sprite = new SpriteSheet(defaultAssetName, sheetIndex);
        }
       /* public override void UpdateGraphicsToMatchSurroundings()
        {
            //TODO: choose hor/ver wall
        }*/
    }


    class KeyTile : FloorTile
    {
        public KeyTile(int layer = 0, string id = "") : base("floorTile", TileType.Key, layer, id)
        {
            secondarySprite = new SpriteSheet("keyOverlay");
            secondarySpriteColor = Color.Aqua;
        }

        public override InventoryItem GetItem()
        {
            InventoryItem key = new InventoryKey();
            return key;
        }

        public override void UpdateGraphics()
        {
            if (isVisited)
                sprite = new SpriteSheet("FloorTile");
        }

        //TODO: if key has been picked up, keytile changes into floortile.
    }
}
