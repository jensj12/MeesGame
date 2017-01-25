using System;

namespace MeesGame
{
    class PlayingLevel : Level
    {
        public PlayingLevel(TileField tileField, int levelindex = 0, int playerIndex = 0, int playerDifficulty = 0)
        {
            start = tileField.Start;
            numRows = tileField.Rows;
            numColumns = tileField.Columns;
            Tiles = tileField;
            Tiles.UpdateGraphicsToMatchSurroundings();
            Tiles.UpdatePortals();
            switch (playerIndex)
            {
                case 0:
                    UseHumanPlayer();
                    break;
                case 1:
                    UseRandomWalkingAIPlayer();
                    break;
                case 2:
                    try
                    {
                        UseAStarAIPlayer();
                    }
                    catch (Exception e)
                    {
                        goto default;
                    }
                    break;
                case 3:
                    UseMonteCarloAIPlayer();
                    break;
                default:
                    UseHumanPlayer();
                    break;
            }
        }

        private void UseMonteCarloAIPlayer()
        {
            usePlayer(new AIPlayer(new AI.MonteCarlo(), this, start));
        }

        private void UseAStarAIPlayer()
        {
            usePlayer(new AIPlayer(new AI.AStar(), this, start));
        }

        public void UseHumanPlayer()
        {
            usePlayer(new HumanPlayer(this, start));
        }

        public void UseRandomWalkingAIPlayer()
        {
            usePlayer(new AIPlayer(new AI.RandomWalker(), this, start));
        }

        public PlayerGameObject Player
        {
            get { return player as PlayerGameObject; }
        }
    }
}
