using MeesGame;
using Microsoft.Xna.Framework;

namespace MeesGen
{
    partial class MazeGenerator
    {
        /// <summary>
        /// Generates a random maze using backtracking. Use odd dimensions for best effect (even dimensions will be extended).
        /// </summary>
        /// <param name="numRows">The number of rows the generated maze should have</param>
        /// <param name="numCols">The number of columns the generated maze should have</param>
        /// <param name="difficulty">The difficulty if the generated maze. 1-5</param>
        /// <returns>The populated TileField</returns>
        public static TileField GenerateMaze(int numRows = DEFAULT_NUM_ROWS, int numCols = DEFAULT_NUM_COLS, int difficulty = 3)
        {
            MazeGenerator mazeGen = new MazeGenerator();
            mazeGen.InitMazeGen(numRows, numCols, difficulty);
            mazeGen.MazeGenMain();
            return mazeGen.Finish();
        }

        private void MazeGenMain()
        {
            // Add hallways until all tiles are filled (main algorithm).
            while (nodesToDo.Count != 0)
            {
                nodesDone += 1;

                // Continue from the last node, or take a random one
                DetermineNextPositionToExpandFrom();

                Point current = nodesToDo[position];
                Point next;

                // Make an arrray and fill it with 0-3 in a random order.
                int[] possible = getZeroToThreeInRandomOrder();

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
                                // Add a tile between the hallways.
                                tileToAdd = ChooseLoopTile();
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
                    // It also has to be next to the edge of the tilefield, next will never be on the edge itself.
                    if (exitScore[next.X, next.Y] > exitScore[bestExit.X, bestExit.Y] && tiles.NearEdgeOfTileField(next.X, next.Y))
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

            // Add an end tile on the edge next to the best exit.
            bestExit = FindEdgeTile(bestExit);
            tiles.Add(new EndTile(), bestExit.X, bestExit.Y);
        }
    }
}
