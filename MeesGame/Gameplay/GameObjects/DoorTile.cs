namespace MeesGame
{
    class DoorTile : WallTile
    {
        public const string defaultAssetName = "horizontalDoorOverlay";
        protected bool doorIsOpen = false;

        //TODO: Pair a door with a specific key.
        public DoorTile(int layer = 0, string id = "") : base("horizontalDoorOverlay", TileType.Door, layer, id)
        {
        }

        //TODO implement doorIsOpen
        public override bool CanPlayerMoveHere(Player player)
        {
            //A player is allowed to move onto a door tile if the door is open.
            //if (DoorIsOpen(player))
            if (player.HasKey())
                if (doorIsOpen)
                    return true;
                else
                    return false;
            return true;
        }

        /*TODO 
        public bool DoorIsOpen(Player player)
        public void OpenDoor(Player player)
        {
            //A player is allowed to open a door if he has the right key.
            bool heeftSleutel = player.PlayerHasKey();
            return false;
        }*/
            //A player is only allowed to open a door if he has the right key.
            if (player.HasKey())
            {
                doorIsOpen = true;
            }
}

public override void UpdateGraphicsToMatchSurroundings()
{
@@ -63,11 + 68,21 @@ namespace MeesGame
{
    public const string defaultAssetName = "keyOverlay";

<<<<<<< HEAD
         public KeyTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Key, layer, id)
=======
        public KeyTile(int layer = 0, string id = "") : base("keyOverlay", TileType.Key, layer, id)
>>>>>>> refs/remotes/origin/master
        {

        }

public override InventoryItem GetItem()
{
    InventoryItem key = new InventoryKey();
    return key;
}

        //TODO: if key has been picked up, keytile changes into floortile.
    }
}