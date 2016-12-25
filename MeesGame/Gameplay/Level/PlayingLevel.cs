using Microsoft.Xna.Framework;

namespace MeesGame
{
    class PlayingLevel : Level
    {
        public PlayingLevel(TileField tileField, int levelindex = 0, bool human = true)
        {
            start = tileField.Start;
            numRows = tileField.Rows;
            numColumns = tileField.Columns;
            Tiles = tileField;
            Tiles.UpdateGraphicsToMatchSurroundings();
            if (human)
            {
                UseHumanPlayer();
            }
        }

        public void UseHumanPlayer()
        {
            usePlayer(new HumanPlayer(this, start));
        }

        public void UseTimedPlayer()
        {
            usePlayer(new TimedPlayer(this, start));
        }

        public void UseUntimedPlayer()
        {
            usePlayer(new Player(this, start));
        }
    }
}