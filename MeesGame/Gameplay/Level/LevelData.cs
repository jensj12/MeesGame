namespace MeesGame
{
    class LevelData
    {
        public int numRows;
        public int numColumns;
        public Tile[,] tilefield;

        public TileField GetData(string filename)
        {
            LevelData data = FileIO.Load(filename);
            TileField tilefield = new TileField(numRows, numColumns);
            this.tilefield = data.tilefield;
            for (int x = 0; x <= numColumns; x++)
            {
                for (int y = 0; y <= numRows; y++)
                {
                    tilefield.Add(this.tilefield[x, y], x, y);
                }
            }
            return tilefield;
        }

        public void SetData(TileField tilefield)
        {
            int rows = tilefield.NumRows;
            int columns = tilefield.NumColumns;
            this.tilefield = new Tile[rows, columns];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    this.tilefield[x, y] = tilefield.GetTile(x, y);
                }
            }
        }
    }
}
