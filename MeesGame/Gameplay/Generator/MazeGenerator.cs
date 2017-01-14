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
            double loopChance = 0, loopDoorChance = 0, loopHoleChance = 0, splitChance = 0, doorChance = 0;
            int numKeys = 0;
            difficulty = MathHelper.Clamp(difficulty, 1, 5);
            switch (difficulty)
            {
                case 1:
                    loopChance = 0.06;
                    loopDoorChance = 0.45;
                    loopHoleChance = 0.15;
                    splitChance = 0.03;
                    doorChance = 0;
                    numKeys = 3;
                    break;
                case 2:
                    loopChance = 0.04;
                    loopDoorChance = 0.7;
                    loopHoleChance = 0.1;
                    splitChance = 0.04;
                    doorChance = 0.01;
                    numKeys = 2;
                    break;
                case 3:
                    loopChance = 0.03;
                    loopDoorChance = 0.8;
                    loopHoleChance = 0.1;
                    splitChance = 0.03;
                    doorChance = 0.03;
                    numKeys = 2;
                    break;
                case 4:
                    loopChance = 0.02;
                    loopDoorChance = 0.8;
                    loopHoleChance = 0.15;
                    splitChance = 0.03;
                    doorChance = 0.05;
                    numKeys = 1;
                    break;
                case 5:
                    loopChance = 0.01;
                    loopDoorChance = 0.3;
                    loopHoleChance = 0.7;
                    splitChance = 0.02;
                    doorChance = 0.08;
                    numKeys = 1;
                    break;
            }

            // Make sure the dimensions of the maze are add and reasonable.
            numRows = MathHelper.Clamp(numRows, 7, 101);
            numCols = MathHelper.Clamp(numCols, 7, 101);
            if (numRows % 2 == 0) numRows += 1;
            if (numCols % 2 == 0) numCols += 1;

            // Create a tilefield filled with walls with the specified dimensions.
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

            // Keys are placed in a specified step in the generating process, so first determine which step
            int[] keyNodes = new int[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                do
                {
                    keyNodes[i] = (random.Next(totalNodesToDo) + random.Next(totalNodesToDo)) / 2; // Try to get it halfway.
                } while (isDuplicate(keyNodes, i)); // Every value has to be different
            }

            // PlaceDoors is false at first because there needs to be a key first.
            bool placeDoors = false;

            // Decide a random point to start.
            Point start = new Point(random.Next(tiles.Columns / 2 - 1) * 2 + 1, random.Next(tiles.Rows / 2 - 1) * 2 + 1);
            nodesToDo.Add(start);
            
            // The exit will be placed at the hardest to reach tile, so keep an array for that
            int[,] exitScore = new int[numRows, numCols];
            Point bestExit;
            bestExit = start;

            int nodesDone = -1; // Make sure it's 0 in the first iteration.

            // If randomNext the algorithm will continue from a random point instead of finishing the path it's currently on, creating a split.
            bool randomNext = false;
            
            Tile tileToAdd;

            // Add hallways until all tiles are filled (main algorithm).
            while (nodesToDo.Count != 0)
            {
                nodesDone += 1;

                // Continue from the last node, or take a random one
                int position = nodesToDo.Count - 1;
                if (randomNext || random.Next(BIGINT) < splitChance * BIGINT)
                {
                    // Swap the last node with another node
                    int nodeToDoNext = random.Next(nodesToDo.Count);
                    swap(nodesToDo, nodeToDoNext, position);
                }
                
                Point current = nodesToDo[position];
                Point next;

                // Make an arrray and fill it with 1-4 in a random order.
                int[] possible = getOneToFourInRandomOrder();

                // Set randomNext to true, it gets set to false if we can extend the hallway
                randomNext = true;

                for (int i = 0; i < 4; i++)
                {
                    // Decide the next point.
                    next = decideNext(possible[i], current);

                    // Check if it's in the tilefield.
                    if (next.X < 0 || next.Y < 0 || next.X >= tiles.Columns || next.Y >= tiles.Rows) continue;

                    // If it's not a walltile.
                    if (!(tiles.GetTile(next) is WallTile))
                    {
                        // Making a path there would create a loop.
                        if (random.Next(BIGINT) < loopChance * BIGINT)
                        {
                            // If the wall inbetween is a walltile.
                            if (tiles.GetTile((next.X + current.X) / 2, (next.Y + current.Y) / 2) is WallTile)
                            {
                                // Make the loop.
                                tileToAdd = new FloorTile();

                                // Or add a door instead of making it a loop, you don't need to check if a key has been placed for this door because it's placed in a loop.
                                if (random.Next(BIGINT) < loopDoorChance * BIGINT)
                                {
                                    tileToAdd = new DoorTile();
                                }

                                // Add the tile.
                                int randNumb = random.Next(BIGINT);
                                if (randNumb < (loopDoorChance + loopHoleChance) * BIGINT) tileToAdd = new DoorTile();
                                if (randNumb < loopHoleChance * BIGINT) tileToAdd = new HoleTile();
                                tiles.Add(tileToAdd, (next.X + current.X) / 2, (next.Y + current.Y) / 2);
                            }
                        }
                        // Next is not a wall, so continue another direction.
                        continue;
                    }

                    // Current and the tile inbetween current and next have been done, so continue on from next.
                    nodesToDo.Add(next);

                    // Add the path to the next tile, and check if keys/doors need to be placed.
                    tileToAdd = new FloorTile();
                    foreach (int keyNode in keyNodes)
                    {
                        if (nodesDone == keyNode)
                        {
                            // Once a key has been placed, a door can be placed.
                            tileToAdd = new KeyTile();
                            placeDoors = true;
                        }
                    }
                    tiles.Add(tileToAdd, next.X, next.Y);

                    // Exitscore is higher the further you are from the start, following the paths.
                    exitScore[next.X, next.Y] = exitScore[current.X, current.Y] + 1;

                    // Re-initialise tileToAdd with the default, a floor tile
                    tileToAdd = new FloorTile();

                    // If a key has been placed, placeDoors is true and there is a chance to place doors.
                    if (placeDoors && random.Next(BIGINT) < doorChance * BIGINT)
                    {
                        tileToAdd = new DoorTile();
                        // If there's a door between the tiles, it's more difficult to get from one to another, so the exit score should increase by more.
                        exitScore[next.X, next.Y] = exitScore[current.X, current.Y] + 10;
                    }
                    tiles.Add(tileToAdd, (next.X + current.X) / 2, (next.Y + current.Y) / 2);

                    // The best exit is the one hardest to reach from the start.
                    if (exitScore[next.X, next.Y] > exitScore[bestExit.X, bestExit.Y])
                    {
                        bestExit = next;
                    }

                    // We placed another hallway, so try to continue from there in the next iteration
                    randomNext = false;
                    break;
                }

                if (randomNext)
                {
                    // We can't extend any further from this tile, so remove it from the list
                    nodesToDo.RemoveAt(position);
                }
            }

            // Add the startTile.
            tiles.Add(new StartTile(0, "playerstart"), start.X, start.Y);

            // Add an end tile on the best exit.
            tiles.Add(new EndTile(), bestExit.X, bestExit.Y);
            return tiles;
        }



        // Helper functions to keep the code above clean.

        /// <summary>
        /// Decide the next point from the current point
        /// </summary>
        /// <param name="possible">the direction to the next point</param>
        /// <param name="current">the current point</param>
        /// <returns>the next point</returns>
        private static Point decideNext(int direction, Point current)
        {
            switch (direction)
            {
                case 1:
                    return new Point(current.X - 2, current.Y);
                case 2:
                    return new Point(current.X + 2, current.Y);
                case 3:
                    return new Point(current.X, current.Y - 2);
                case 4:
                    return new Point(current.X, current.Y + 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Swap 2 items in a list
        /// </summary>
        /// <param name="list">a list</param>
        /// <param name="pos1">item 1</param>
        /// <param name="pos2">item 2</param>
        private static void swap(IList<Point> list, int pos1, int pos2)
        {
            Point save = list[pos1];
            list[pos1] = list[pos2];
            list[pos2] = save;
        }

        /// <summary>
        /// create an array filled with 1-4 in a random order
        /// </summary>
        /// <returns>Array containing 1 to 4 in a random order</returns>
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

        /// <summary>
        /// Returns whether a newElement is already an earlier item in the list
        /// </summary>
        /// <param name="list">a list</param>
        /// <param name="newElement">the new element</param>
        /// <returns>if the new element is already an item somewhere earlier in the list</returns>
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