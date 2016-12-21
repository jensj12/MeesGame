using Microsoft.Xna.Framework;

namespace MeesGame
{
    class EditorLevel : Level
    {
        private const string tileFieldName = "editorTiles";

        public EditorLevel(int levelindex = 0, int screenWidth = -1, int screenHeight = -1) : base(false)
        {
            start = Point.Zero;
            FillLevelWithEmptyTiles();
            Tiles.UpdateGraphicsToMatchSurroundings();
            usePlayer(new EditorPlayer(this, start), screenWidth, screenHeight);
        }

        public void FillLevelWithEmptyTiles()
        {
            for (int x = 0; x < numRows; x++)
                for (int y = 0; y < numColumns; y++)
                    Tiles.Add(new FloorTile(), x, y);
        }

        public void FillLevelWithTiles(TileField tiles)
        {
            for (int x = 0; x < numRows; x++)
                for (int y = 0; y < numColumns; y++)
                    this.Tiles.Add(tiles.Objects[x,y], x, y);
        }
    }
}