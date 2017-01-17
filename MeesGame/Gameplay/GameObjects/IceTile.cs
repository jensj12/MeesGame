namespace MeesGame
{
    class IceTile : FloorTile
    {
        private Direction lastDirection;

        public IceTile(int layer = 0, string id = "") : base(TileType.Ice, 0, id)
        {

        }

        public override void EnterTile(ITileFieldPlayer player)
        {
            base.EnterTile(player);
            lastDirection = player.LastAction.ToDirection();
        }

        public override bool IsActionForbiddenFromHere(ITileFieldPlayer player, CharacterAction action)
        {
            if (!TileField.GetTile(Location + lastDirection.ToPoint()).StopsSliding)
                return false;

            return base.IsActionForbiddenFromHere(player, action);
        }

        public override void PerformAction(ITileFieldPlayer player, CharacterAction action)
        {
            if (!TileField.GetTile(Location + lastDirection.ToPoint()).StopsSliding)
            {
                player.MoveSmoothly(lastDirection);
            }
            else
            {
                base.PerformAction(player, action);
            }
        }
    }
}
