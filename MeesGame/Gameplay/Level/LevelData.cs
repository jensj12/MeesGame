namespace MeesGame
{
    public class LevelData
    {
        public int numRows;
        public int numColumns;
        public TileData[][] tilefield;

        public TileField GetData(string filename)
        {
            LevelData data = FileIO.Load(filename);
            TileField tilefield = new TileField(numRows, numColumns);
            this.tilefield = data.tilefield;
            for (int x = 0; x <= numColumns; x++)
            {
                for (int y = 0; y <= numRows; y++)
                {
                    tilefield.Add(Tile.CreateTileFromTileData(this.tilefield[x][y]), x, y);
                }
            }
            return tilefield;
        }

        public void SetData(TileField tilefield)
        {
            numRows = tilefield.Rows;
            numColumns = tilefield.Columns;
            this.tilefield = new TileData[numColumns][];
            for (int x = 0; x < numColumns; x++)
            {
                this.tilefield[x] = new TileData[numRows];
                for (int y = 0; y < numRows; y++)
                {
                    this.tilefield[x][y] = tilefield.GetTile(x, y).Data;
                }
            }
        }
    }
}
