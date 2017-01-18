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

        private double loopChance = 0;
        private double loopDoorChance = 0;
        private double loopHoleChance = 0;
        private double splitChance = 0;
        private double doorChance = 0;
        private TileField tiles;
        private Point bestExit;
        private int[,] exitScore;
        private IList<Point> nodesToDo;
        private int totalNodesToDo;
        private int[] keyNodes;
        private Point start;
        private bool placeDoors;
        private bool randomNext;
        private Tile tileToAdd;
        private int nodesDone;
        private int position;
        private int numKeys;
        private int keysPlaced, doorsPlaced;
        private int numRows, numCols;

        private void InitMazeGen(int numRows, int numCols, int difficulty)
        {
            InitialiseVariables(numRows, numCols);
            SetDifficultyParameters(difficulty);
            InitialiseTileField();
            InitialiseKeys();
            InitialiseStart();
            InitialiseExitSearch();
        }

        private void SetDifficultyParameters(int difficulty)
        {
            difficulty = MathHelper.Clamp(difficulty, 1, 5);
            switch (difficulty)
            {
                case 1:
                    loopChance = 0.04;
                    loopDoorChance = 0.2;
                    loopHoleChance = 0.55;
                    splitChance = 0.03;
                    doorChance = 0;
                    numKeys = 2;
                    break;
                case 2:
                    loopChance = 0.03;
                    loopDoorChance = 0.2;
                    loopHoleChance = 0.6;
                    splitChance = 0.04;
                    doorChance = 0.01;
                    numKeys = 3;
                    break;
                case 3:
                    loopChance = 0.025;
                    loopDoorChance = 0.2;
                    loopHoleChance = 0.7;
                    splitChance = 0.03;
                    doorChance = 0.03;
                    numKeys = 4;
                    break;
                case 4:
                    loopChance = 0.02;
                    loopDoorChance = 0.2;
                    loopHoleChance = 0.75;
                    splitChance = 0.03;
                    doorChance = 0.05;
                    numKeys = 5;
                    break;
                case 5:
                    loopChance = 0.01;
                    loopDoorChance = 0.1;
                    loopHoleChance = 0.9;
                    splitChance = 0.02;
                    doorChance = 0.08;
                    numKeys = 6;
                    break;
            }
        }

        private void ValidateDimensions()
        {
            // Make sure the dimensions of the maze are odd and reasonable.
            numRows = MathHelper.Clamp(numRows, 7, 101);
            numCols = MathHelper.Clamp(numCols, 7, 101);
            if (numRows % 2 == 0) numRows += 1;
            if (numCols % 2 == 0) numCols += 1;
        }

        private void InitialiseTileField()
        {
            ValidateDimensions();

            // Create a tilefield filled with walls with the specified dimensions.
            tiles = new TileField(numRows, numCols);
            for (int y = 0; y < tiles.Columns; y++)
            {
                for (int x = 0; x < tiles.Rows; x++)
                {
                    tiles.Add(new WallTile(), x, y);
                }
            }
        }

        private void InitialiseVariables(int numRows, int numCols)
        {
            this.numRows = numRows;
            this.numCols = numCols;
            nodesToDo = new List<Point>();

            // Only place doors after a key has been placed.
            placeDoors = false;

            // Is set to true if the current path can't be extended.
            randomNext = false;

            // Makes it 0 in the first iteration.
            nodesDone = -1;
        }

        private void InitialiseKeys()
        {
            totalNodesToDo = (tiles.Rows - 1) * (tiles.Columns - 1) / 4;

            // Keys are placed in a specified step in the generating process, so first determine which step.
            keyNodes = new int[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                do
                {
                    keyNodes[i] = (random.Next(totalNodesToDo) + random.Next(totalNodesToDo)) / 2; // Try to get it halfway.
                } while (isDuplicate(keyNodes, i)); // Every value has to be different.
            }
        }

        private void InitialiseStart()
        {
            // Decide a random point to start.
            start = new Point(random.Next(tiles.Columns / 2 - 1) * 2 + 1, random.Next(tiles.Rows / 2 - 1) * 2 + 1);
            nodesToDo.Add(start);
        }

        private void InitialiseExitSearch()
        {
            // The exit will be placed at the hardest to reach tile, so keep an array for that.
            exitScore = new int[numRows, numCols];
            bestExit = start;
        }
    }
}
