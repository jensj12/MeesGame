using Microsoft.Xna.Framework;

namespace MeesGame
{
    class PlayingLevel : Level
    {
        public PlayingLevel(int levelindex = 0, bool human = true)
        {
            start = new Point(GameEnvironment.Random.Next(tiles.Columns / 2) * 2, GameEnvironment.Random.Next(tiles.Rows / 2) * 2);
            tiles = MeesGen.MazeGenerator.GenerateMaze(tiles, start.X, start.Y);
            tiles.UpdateGraphicsToMatchSurroundings();
            tiles.UpdateGraphics();
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