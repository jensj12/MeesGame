using Microsoft.Xna.Framework;

namespace MeesGame
{
    class EditorLevel : Level
    {
        private const string tileFieldName = "editorTiles";

        public EditorLevel(int levelindex = 0, int screenWidth = -1, int screenHeight = -1)
        {
            tiles = new TileField(numRows, numColumns, false, 0, "tiles");
            tiles.CellHeight = CELL_HEIGHT;
            tiles.CellWidth = CELL_WIDTH;

            start = Point.Zero;
            FillLevelWithEmptyTiles();
            tiles.UpdateGraphicsToMatchSurroundings();
            usePlayer(new EditorPlayer(this, start), screenWidth, screenHeight);
        }

        private void FillLevelWithEmptyTiles()
        {
            for (int x = 0; x < numRows; x++)
                for (int y = 0; y < numColumns; y++)
                    tiles.Add(new FloorTile(), x, y);
        }
    }
}
