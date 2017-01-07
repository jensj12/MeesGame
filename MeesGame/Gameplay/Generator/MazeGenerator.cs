using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MeesGen
{
    class MazeGenerator
    {
        private const int BIGINT = 100000;
        private const int DEFAULT_NUM_ROWS = Level.DEFAULT_NUM_ROWS;
        private const int DEFAULT_NUM_COLS = Level.DEFAULT_NUM_COLS;

        /// <summary>
        /// Generates a random maze using backtracking. Use odd dimensions for best effect (even dimensions will be extended).
        /// </summary>
        /// <param name="numRows">The number of rows the generated maze should have</param>
        /// <param name="numCols">The number of columns the generated maze should have</param>
        /// <param name="difficulty">The difficulty if the generated maze. 1-5</param>
        /// <returns>The populated TileField</returns>
        public static TileField GenerateMaze(int numRows = DEFAULT_NUM_ROWS, int numCols = DEFAULT_NUM_COLS, int difficulty = 3)
        {
            double loopChance = 0, loopDoorChance = 0, splitChance = 0, doorChance = 0;
            int numKeys = 0;
            difficulty = MathHelper.Clamp(difficulty, 1, 5);
            switch (difficulty)
            {
                case 1:
                    loopChance = 0.05;
                    loopDoorChance = 0.5;
                    splitChance = 0.03;
                    doorChance = 0;
                    numKeys = 3;
                    break;
                case 2:
                    loopChance = 0.04;
                    loopDoorChance = 0.8;
                    splitChance = 0.04;
                    doorChance = 0.01;
                    numKeys = 2;
                    break;
                case 3:
                    loopChance = 0.03;
                    loopDoorChance = 0.9;
                    splitChance = 0.03;
                    doorChance = 0.03;
                    numKeys = 2;
                    break;
                case 4:
                    loopChance = 0.02;
                    loopDoorChance = 0.95;
                    splitChance = 0.03;
                    doorChance = 0.05;
                    numKeys = 1;
                    break;
                case 5:
                    loopChance = 0.005;
                    loopDoorChance = 1;
                    splitChance = 0.02;
                    doorChance = 0.08;
                    numKeys = 1;
                    break;
            }

            numRows = MathHelper.Clamp(numRows, 7, 101);
            numCols = MathHelper.Clamp(numCols, 7, 101);
            if (numRows % 2 == 0) numRows += 1;
            if (numCols % 2 == 0) numCols += 1;

            TileField tiles = new TileField(numRows, numCols);

            for (int y = 0; y < tiles.Columns; y++)
            {
                for (int x = 0; x < tiles.Rows; x++)
                {
                    tiles.Add(new WallTile(), x, y);
                }
            }

            IList<Point> nodesToDo = new List<Point>();
            int totalNodesToDo = (tiles.Rows - 1) * (tiles.Columns - 1) / 4;

            int[] keyNodes = new int[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                do
                {
                    keyNodes[i] = (random.Next(totalNodesToDo) + random.Next(totalNodesToDo)) / 2; //try to get it halfway
                } while (isDuplicate(keyNodes, i));
            }
            bool placeDoors = false;

            Point start = new Point(random.Next(tiles.Columns / 2 - 1) * 2 + 1, random.Next(tiles.Rows / 2 - 1) * 2 + 1);
            nodesToDo.Add(start);
            tiles.Add(new StartTile(0, "playerstart"), start.X, start.Y);

            int[,] exitScore = new int[numRows, numCols];
            Point bestExit;
            bestExit = start;

            int nodesDone = -1; //make sure it's 0 in the first iteration
            bool randomNext = false;
            Tile tileToAdd;

            // Add hallways until all tiles are filled (main algorithm)
            while (nodesToDo.Count != 0)
            {
                nodesDone += 1;
                int position = nodesToDo.Count - 1;
                if (randomNext || random.Next(BIGINT) < splitChance * BIGINT)
                {
                    int nodeToDoNext = random.Next(nodesToDo.Count);
                    swap(nodesToDo, nodeToDoNext, position);
                }
                Point current = nodesToDo[position];
                Point next;
                int[] possible = getOneToFourInRandomOrder(); //possible now contains 1-4 in random order

                randomNext = true;
                for (int i = 0; i < 4; i++)
                {
                    switch (possible[i])
                    {
                        case 1:
                            next = new Point(current.X - 2, current.Y);
                            break;
                        case 2:
                            next = new Point(current.X + 2, current.Y);
                            break;
                        case 3:
                            next = new Point(current.X, current.Y - 2);
                            break;
                        case 4:
                            next = new Point(current.X, current.Y + 2);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                            //break;
                    }

                    if (next.X < 0 || next.Y < 0 || next.X >= tiles.Columns || next.Y >= tiles.Rows) continue;
                    if (!(tiles.GetTile(next) is WallTile))
                    {
                        if (random.Next(BIGINT) < loopChance * BIGINT)
                        {
                            if (tiles.GetTile((next.X + current.X) / 2, (next.Y + current.Y) / 2) is WallTile)
                            {
                                tileToAdd = new FloorTile();
                                if (random.Next(BIGINT) < loopDoorChance * BIGINT)
                                {
                                    tileToAdd = new DoorTile();
                                }
                                tiles.Add(tileToAdd, (next.X + current.X) / 2, (next.Y + current.Y) / 2);
                            }
                        }
                        continue;
                    }

                    nodesToDo.Add(next);

                    // Add the path to the next tile, and check if keys/doors need to be placed
                    tileToAdd = new FloorTile();
                    foreach (int keyNode in keyNodes)
                    {
                        if (nodesDone == keyNode)
                        {
                            tileToAdd = new KeyTile();
                            placeDoors = true;
                        }
                    }
                    tiles.Add(tileToAdd, next.X, next.Y);
                    exitScore[next.X, next.Y] = exitScore[current.X, current.Y] + 1;

                    tileToAdd = new FloorTile();
                    if (placeDoors && random.Next(BIGINT) < doorChance * BIGINT)
                    {
                        tileToAdd = new DoorTile();
                        exitScore[next.X, next.Y] = exitScore[current.X, current.Y] + 10;
                    }
                    tiles.Add(tileToAdd, (next.X + current.X) / 2, (next.Y + current.Y) / 2);

                    if (exitScore[next.X, next.Y] > exitScore[bestExit.X, bestExit.Y])
                    {
                        bestExit = next;
                    }
                    randomNext = false;
                    break;
                }
                if (randomNext)
                {
                    nodesToDo.RemoveAt(position);
                }
            }

            tiles.Add(new EndTile(), bestExit.X, bestExit.Y);
            return tiles;
        }



        // Helper functions to keep the code above clean

        private static void swap(IList<Point> list, int pos1, int pos2)
        {
            Point save = list[pos1];
            list[pos1] = list[pos2];
            list[pos2] = save;
        }

        private static int[] getOneToFourInRandomOrder()
        {
            int[] ints = { 0, 0, 0, 0 };
            for (int i = 1; i < 5; i++)
            {
                while (true)
                {
                    int pos = random.Next(4);
                    if (ints[pos] != 0) continue;
                    ints[pos] = i;
                    break;
                }
            }
            return ints;
        }

        private static Random random
        {
            get { return GameEnvironment.Random; }
        }

        private static bool isDuplicate(int[] list, int newElement)
        {
            for (int j = 0; j < newElement; j++)
            {
                if (list[newElement] == list[j])
                {
                    return true;
                }
            }
            return false;
        }
    }
}