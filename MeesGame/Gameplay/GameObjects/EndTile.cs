namespace MeesGame
{
    class EndTile : FloorTile
    {
        public EndTile(int layer = 0, string id = "") : base("end_door", TileType.End, layer, id)
        {

        }

        //Check if the player finished the level
        public void Finished(Player player)
        {
            if (player.Position == this.position)
                GameEnvironment.GameStateManager.SwitchTo("GameOverState");
        }
    }
}
