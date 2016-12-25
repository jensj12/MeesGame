namespace MeesGame
{
    public class LevelData
    {
        public int numRows;
        public int numColumns;
        public TileData[][] tilefield;

        public LevelData()
        {

        }

        public LevelData(TileField tileField)
        {
            numRows = tileField.Rows;
            numColumns = tileField.Columns;
            this.tilefield = new TileData[numColumns][];
            for (int x = 0; x < numColumns; x++)
            {
                this.tilefield[x] = new TileData[numRows];
                for (int y = 0; y < numRows; y++)
                {
                    this.tilefield[x][y] = tileField.GetTile(x, y).Data;
                }
            }
        }

        public TileField ToTileField()
        {
            TileField tilefield = new TileField(numRows, numColumns);
            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    tilefield.Add(this.tilefield[x][y], x, y);
                }
            }
            return tilefield;
        }

        public static implicit operator TileField(LevelData data)
        {
            return data.ToTileField();
        }

        public static implicit operator LevelData(TileField tileField)
        {
            return new LevelData(tileField);
        }
    }
}
