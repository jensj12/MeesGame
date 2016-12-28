namespace MeesGame
{
    class FloorTile : Tile
    {
        protected FloorTile(TileType tt = TileType.Floor, int layer = 0, string id = "") : base(tt, layer, id)
        {
        }

        public FloorTile(int layer = 0, string id = "") : base(TileType.Floor, layer, id)
        {
        }

        public override bool CanPlayerMoveHere(Player player)
        {
            //A player is allowed to move onto floors
            return true;
        }

        public override bool IsActionForbiddenFromHere(Player player, PlayerAction action)
        {
            //A player can move into all directions from a floor tile.
            //Special actions are allowed on floor tiles, if there is a tile which allows a special action next to it.
            if (IsNextToSpecialTile())
            {
                return false;
            }
            //Special actions are not allowed on a floor tile, if it is surrounded by tiles that don't allow special actions.
            else
                return action == PlayerAction.SPECIAL;
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
        }
        
        public override void UpdateGraphics()
        {
        }
    }
}
