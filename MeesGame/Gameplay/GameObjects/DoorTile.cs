using Microsoft.Xna.Framework;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class DoorTile : WallTile
    {
        new public const string defaultAssetName = "horizontalDoor";

        protected bool doorIsOpen = false;

        [EditableAttribute]
        public Color Background { get; set; }

        //TODO: Pair a door with a specific key.
        public DoorTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Door, layer, id)
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

        public override void UpdateGraphicsToMatchSurroundings()
        {
            int x = Location.X;
            int y = Location.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile)
            {
                sprite = new SpriteSheet("verticalDoor");
                secondarySprite = new SpriteSheet("verticalDoorOverlay");
            }
            if (tileField.GetTile(x - 1, y) is WallTile)
            {
                sprite = new SpriteSheet("horizontalDoor");
                secondarySprite = new SpriteSheet("horizontalDoorOverlay");
            }
        }


        new public static string[] GetDefaultAssetNames()
        {
            string[] parentDefaultAssetNames = WallTile.GetDefaultAssetNames();
            string[] defaultAssetNames = new string[parentDefaultAssetNames.Length + 1];
            parentDefaultAssetNames.CopyTo(defaultAssetNames, 0);
            defaultAssetNames[parentDefaultAssetNames.Length] = defaultAssetName;
            return defaultAssetNames;
        }
    }


    class KeyTile : FloorTile
    {
        new const string defaultAssetName = "keyOverlay";

        public KeyTile(int layer = 0, string id = "") : base(FloorTile.defaultAssetName, TileType.Key, layer, id)
        {
            secondarySprite = new SpriteSheet(defaultAssetName);
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

        new public static string[] GetDefaultAssetNames()
        {
            string[] parentDefaultAssetNames = FloorTile.GetDefaultAssetNames();
            string[] defaultAssetNames = new string[parentDefaultAssetNames.Length + 1];
            parentDefaultAssetNames.CopyTo(defaultAssetNames, 0);
            defaultAssetNames[parentDefaultAssetNames.Length] = defaultAssetName;
            return defaultAssetNames;
        }

        //TODO: if key has been picked up, keytile changes into floortile.
    }
}