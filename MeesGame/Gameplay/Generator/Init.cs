using MeesGame;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MeesGen
{
    partial class MazeGenerator
    {
        private const int BIGINT = 100000;
        private const int DEFAULT_NUM_ROWS = Level.DEFAULT_NUM_ROWS;
        private const int DEFAULT_NUM_COLS = Level.DEFAULT_NUM_COLS;

        private TileField tiles;
        private Point bestExit;
        private int[,] exitScore;
        private int bestExitScore;
        private behindDoorFlags[,] behindDoorInfo;
        private behindDoorFlags[] keyBehindDoorInfo;
        private IList<Point> nodesToDo;
        private int totalNodesToDo;
        private int[] keyNodes;
        private int[] keyColorOrder;
        private Point start;
        private Point current;
        private Point next;
        private Point pointBetween;
        private bool placeDoors;
        private bool randomNext;
        private bool didAddHallway;
        private Tile tileToAdd;
        private int nodesDone;
        private int position;
        private int keysPlaced;
        private int doorsPlaced;
        private int portalsPlaced;

        private void InitMazeGen(int difficulty)
        {
            SetDifficultyParameters(difficulty);
            InitialiseTileField();
            InitialiseVariables();
            InitialiseKeys();
            InitialiseStart();
        }

        /// <summary>
        /// Make sure the dimensions of the maze are odd and reasonable.
        /// </summary>
        private void ValidateDimensions()
        {
            numRows = MathHelper.Clamp(numRows, 11, 99);
            numCols = MathHelper.Clamp(numCols, 15, 99);
            if (numRows % 2 == 0) numRows += 1;
            if (numCols % 2 == 0) numCols += 1;
        }

        /// <summary>
        /// Create a tilefield filled with walls.
        /// </summary>
        private void InitialiseTileField()
        {
            ValidateDimensions();

            tiles = new TileField(numRows, numCols);
            for (int y = 0; y < tiles.Rows; y++)
            {
                for (int x = 0; x < tiles.Columns; x++)
                {
                    tiles.Add(new WallTile(), x, y);
                }
            }
        }

        private void InitialiseVariables()
        {
            // A list of all nodes that still need to be considered. Generator is finished once this is empty.
            nodesToDo = new List<Point>();

            // Only place doors after a key has been placed.
            placeDoors = false;

            // Is set to true if the current path can't be extended.
            randomNext = false;

            // Makes it 0 in the first iteration.
            nodesDone = -1;

            // The exit will be placed at the hardest to reach tile, so keep an array for that.
            exitScore = new int[numCols, numRows];
            bestExitScore = 0;

            // Keep track of which keys are needed to reach a tile or key
            behindDoorInfo = new behindDoorFlags[numCols, numRows];
            keyBehindDoorInfo = new behindDoorFlags[6];
        }

        /// <summary>
        /// Decides at which steps in the generating process keys are placed
        /// </summary>
        private void InitialiseKeys()
        {
            totalNodesToDo = (tiles.Rows - 1) * (tiles.Columns - 1) / 4;
            // Portals make the algorithm add two tiles in one loop.
            totalNodesToDo -= (int)(totalNodesToDo * portalChance);

            // Keys are placed in a specified step in the generating process, so first determine which step.
            keyNodes = new int[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                do
                {
                    // Try to spread the keys while keeping them away from the exit and start.
                    keyNodes[i] = ((4 * random.Next(totalNodesToDo - 2) + random.Next(totalNodesToDo - 2)) / 5) + 1;
                } while (isDuplicate(keyNodes, i)); // Every value has to be different.
            }

            // Make the order of keys random so blue isn't always the first
            keyColorOrder = Helpers.getZeroToNInRandomOrder(6);
        }

        /// <summary>
        /// Pick a random starting point. Add the start tile to the TileField and as entry point for the main algorithm.
        /// </summary>
        private void InitialiseStart()
        {
            start = new Point(random.Next(tiles.Columns / 2 - 1) * 2 + 1, random.Next(tiles.Rows / 2 - 1) * 2 + 1);
            nodesToDo.Add(start);
            tiles.Add(new StartTile(0, "playerstart"), start.X, start.Y);
            bestExit = start;
        }
    }
}
