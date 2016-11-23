namespace MeesGame
{
    class Level : GameObjectList
    {
        public Level(int levelindex = 0)
        {
            //20 is just a temprary number
            TileField tiles = new TileField(12, 22, 0, "tiles");
            tiles.CellHeight = 64;
            tiles.CellWidth = 64;
            Add(tiles);

            //Temporary initialisation of empty tiles
            for (int x = 0; x < 22; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    tiles.Add(new Tile("tilefloor_nowall"), x, y);
                }
            }
        }
    }
}
