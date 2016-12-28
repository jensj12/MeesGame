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
            else
            {
                UseRandomWalkingAIPlayer();
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

        public void UseRandomWalkingAIPlayer()
        {
            usePlayer(new AIPlayer(new AI.RandomWalker() ,this, start));
        }
    }
}
