using Microsoft.Xna.Framework;

namespace MeesGame
{
    class EditorLevel : Level
    {
        private const string tileFieldName = "editorTiles";

        public EditorLevel(TileField tileField, int levelindex = 0, int screenWidth = -1, int screenHeight = -1)
        {
            start = new Point(1, 1);
            numRows = tileField.Rows;
            numColumns = tileField.Columns;
            Tiles = tileField;
            Tiles.UpdateGraphicsToMatchSurroundings();
            Tiles.FogOfWar = false;
            usePlayer(new EditorPlayer(this, start), screenWidth, screenHeight);
        }

        public static void FillWithEmptyTiles(TileField tf)
        {
            for (int x = 0; x < tf.Columns; x++)
                for (int y = 0; y < tf.Rows; y++)
                    if (tf.OnEdgeOfTileField(x, y))
                        tf.Add(new WallTile(), x, y);
                    else
                        tf.Add(new FloorTile(), x, y);

            tf.UpdateGraphicsToMatchSurroundings();
        }

        public EditorPlayer Player
        {
            get
            {
                return player as EditorPlayer;
            }
        }
    }
}
