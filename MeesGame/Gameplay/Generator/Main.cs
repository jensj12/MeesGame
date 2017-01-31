using MeesGame;
using Microsoft.Xna.Framework;
using System;

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
            mazeGen.InitMazeGen(difficulty);
            mazeGen.MazeGenMain();
            return mazeGen.Finish();
        }

        private void MazeGenMain()
        {
            // Add hallways until all tiles are filled (main algorithm).
            while (nodesToDo.Count != 0)
            {
                // Continue from the last node, or take a random one
                DetermineNextPositionToExpandFrom();

                current = nodesToDo[position];

                // Make an arrray and fill it with 0-3 in a random order.
                int[] possible = Helpers.getZeroToNInRandomOrder(4);

                // Set randomNext to true, it gets set to false if we can extend the hallway
                didAddHallway = false;

                for (int i = 0; i < 4; i++)
                {
                    // Decide the next point.
                    next = decideNext(possible[i], current);

                    // Check if it's in the tilefield.
                    if (next.X < 0 || next.Y < 0 || next.X >= tiles.Columns || next.Y >= tiles.Rows) continue;

                    // If it's not a walltile, the tile has alreade been visited by the generator.
                    if (!(tiles.GetType(next) == TileType.Wall))
                    {
                        // Making a path there would create a loop.
                        if (randomChance(loopChance))
                        {
                            // If the wall inbetween is a walltile.
                            if (tiles.GetType(PointBetween(next, current)) == TileType.Wall)
                            {
                                // Add a tile between the hallways.
                                AddLoopTile(next, current);
                            }
                        }
                        // Next is already a hallway, so continue another direction.
                        continue;
                    }

                    nodesDone += 1;

                    // We can add another hallway, so try to continue from there in the next iteration
                    didAddHallway = true;
                    randomNext = false;
                    pointBetween = PointBetween(current, next);

                    // Current and the tile inbetween current and next have been done, so continue on from next.
                    nodesToDo.Add(next);

                    // Add the two tiles, the tile in between first to prevent a door being placed in front of the corresponding key
                    AddTileInBetween();
                    AddNextTile();
                    UpdateBestExit(next);

                    // Don't continue searching for more pathways from this point
                    break;
                }

                if (!didAddHallway)
                {
                    // We can't extend any further from this tile, so remove it from the list
                    nodesToDo.RemoveAt(position);
                    randomNext = true;
                }
            }

            // Add an end tile on the edge next to the best exit.
            bestExit = FindEdgeTile(bestExit);
            tiles.Add(new EndTile(), bestExit.X, bestExit.Y);
        }

        /// <summary>
        /// Add a tile on the next tile, next to the tileInBetween
        /// </summary>
        private void AddNextTile()
        {
            //Add the path to the next tile, and check if keys / doors need to be placed.
            tileToAdd = new FloorTile();

            // Add a key if the time has come
            foreach (int keyNode in keyNodes)
            {
                if (nodesDone == keyNode)
                {
                    // Once a key has been placed, a door can be placed.
                    tileToAdd = new KeyTile();
                    (tileToAdd as KeyTile).SecondarySpriteColor = ChooseColor(keysPlaced);
                    keyBehindDoorInfo[keysPlaced] = behindDoorInfo[next.X, next.Y];
                    foreach (behindDoorFlags flag in Enum.GetValues(typeof(behindDoorFlags)))
                    {
                        if ((flag & behindDoorInfo[next.X,next.Y]) != 0)
                            keyBehindDoorInfo[keysPlaced] |= keyBehindDoorInfo[(int)Math.Log((int)flag, 2)];
                    }
                    keysPlaced++;
                    placeDoors = true;
                    // Choosing a random position next time generally makes it harder to find keys
                    randomNext = true;
                }
            }

            // Exitscore is higher the further you are from the start, following the paths.
            UpdateBestExitScore(current, 1, next);

            // Only place portals if we haven't placed a key
            if (randomChance(portalChance) && !randomNext)
            {
                Point secondPortalPosition = DetermineSecondPortalPosition();
                if (tiles.GetType(secondPortalPosition) == TileType.Wall)
                {
                    tileToAdd = new PortalTile();
                    (tileToAdd as PortalTile).PortalIndex = portalsPlaced;
                    tiles.Add(tileToAdd, secondPortalPosition.X, secondPortalPosition.Y);
                    tileToAdd = new PortalTile();
                    (tileToAdd as PortalTile).PortalIndex = portalsPlaced;
                    // Tile will be added below

                    portalsPlaced++;
                    nodesToDo.Add(secondPortalPosition);
                    UpdateBestExitScore(next, portalScore, secondPortalPosition);
                    UpdateBestExit(secondPortalPosition);
                    behindDoorInfo[secondPortalPosition.X, secondPortalPosition.Y] = behindDoorInfo[next.X, next.Y];
                }
            }

            tiles.Add(tileToAdd, next.X, next.Y);
        }

        private void AddTileInBetween()
        {
            // Default is a FloorTile
            tileToAdd = new FloorTile();

            // If a key has been placed, immediately place a door
            if (doorsPlaced < keysPlaced)
            {
                tileToAdd = new DoorTile();
                (tileToAdd as DoorTile).SecondarySpriteColor = ChooseColor(doorsPlaced);
                doorsPlaced++;
            }
            // Always have a chance to place a door after a key has been placed
            else if (placeDoors && randomChance(doorChance))
            {
                tileToAdd = new DoorTile();
                (tileToAdd as DoorTile).SecondarySpriteColor = ChooseColor(keysPlaced, true, true);
            }
            tiles.Add(tileToAdd, pointBetween.X, pointBetween.Y);

            behindDoorInfo[next.X, next.Y] = behindDoorInfo[current.X, current.Y];
            if (tiles.GetType(pointBetween) == TileType.Door)
            {
                // If a door is placed, next is behind all the doors it is currently behind as well as the newly placed door.
                behindDoorInfo[next.X, next.Y] |= GetFlagFromColor((tiles.GetTile(pointBetween) as DoorTile).SecondarySpriteColor);
            }
        }

        /// <summary>
        /// Places a tile between the two points, creating the loop.
        /// </summary>
        private void AddLoopTile(Point one, Point two)
        {
            tileToAdd = new FloorTile();
            behindDoorFlags difference = behindDoorInfo[one.X, one.Y] ^ behindDoorInfo[two.X, two.Y];
            // Check if the difference is exactly one door color
            if (Enum.IsDefined(typeof(behindDoorFlags), difference))
            {
                tileToAdd = new DoorTile();
                (tileToAdd as DoorTile).SecondarySpriteColor = ChooseColor((int)Math.Log((int)difference, 2), false, false);
            }
            if ((!Enum.IsDefined(typeof(behindDoorFlags), difference) && difference != 0) || randomChance(loopHoleChance))
            {
                tileToAdd = new HoleTile();
                if (randomChance(0.5))
                    tileToAdd = new GuardTile();
            }
            tiles.Add(tileToAdd, PointBetween(one, two).X, PointBetween(one, two).Y);
        }
    }
}
