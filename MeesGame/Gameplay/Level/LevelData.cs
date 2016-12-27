namespace MeesGame
{
    public class LevelData
    {
        public int numRows;
        public int numColumns;
        public TileData[][] tileData;

        public LevelData()
        {

        }

        public LevelData(TileField tileField)
        {
            numRows = tileField.Rows;
            numColumns = tileField.Columns;
            tileData = new TileData[numColumns][];
            for (int x = 0; x < numColumns; x++)
            {
                tileData[x] = new TileData[numRows];
                for (int y = 0; y < numRows; y++)
                {
                    tileData[x][y] = tileField.GetTile(x, y).Data;
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
                    tilefield.Add(tileData[x][y], x, y);
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
