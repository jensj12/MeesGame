﻿using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MeesGen
{
    partial class MazeGenerator
    {
        /// <summary>
        /// Decide the next point from the current point, taking two steps from the current point.
        /// </summary>
        /// <param name="dir">The direction to the next point (an int between 0 and 3).</param>
        /// <param name="current">the current point</param>
        /// <returns>The next point</returns>
        private static Point decideNext(int dir, Point current)
        {
            Direction direction = (Direction)dir;
            return current + direction.ToPoint() + direction.ToPoint();
        }

        /// <summary>
        /// Swap 2 items in a list.
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
        /// Create an array filled with 0-3 in a random order.
        /// </summary>
        /// <returns>Array containing 0 to 3 in a random order.</returns>
        private static int[] getZeroToThreeInRandomOrder()
        {
            int[] ints = { -1, -1, -1, -1 };
            for (int i = 0; i < 4; i++)
            {
                while (true)
                {
                    int pos = random.Next(4);
                    if (ints[pos] != -1) continue;
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
        /// Returns whether a newElement is already an earlier item in the list.
        /// </summary>
        /// <param name="list">A list.</param>
        /// <param name="newElement">The location of the element in the list.</param>
        /// <returns>If the new element is already an item somewhere earlier in the list</returns>
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

        private void DetermineNextPositionToExpandFrom()
        {
            position = nodesToDo.Count - 1;
            if (randomNext || random.Next(BIGINT) < splitChance * BIGINT)
            {
                // Swap the last node with another node
                int nodeToDoNext = random.Next(nodesToDo.Count);
                swap(nodesToDo, nodeToDoNext, position);
            }
        }

        private Tile ChooseLoopTile()
        {
            int randNumb = random.Next(BIGINT);
            if (randNumb < loopDoorChance * BIGINT) return new DoorTile();
            // Since the function already returns if the loop is a door, add the chance to the loopHoleChance
            if (randNumb < (loopHoleChance + loopDoorChance) * BIGINT)
            {
                // Guards and Holes have the same effect, so they can be placed on the same way.
                if (random.Next(2) == 1)
                {
                    return new HoleTile();
                }
                else
                {
                    return new GuardTile();
                }
            }

            return new FloorTile();
        }

        private Color ChooseColor(int max, bool randomly = false)
        {
            if (randomly)
            {
                Random randomNumber = new Random();
                max = (int)(randomNumber.Next(0, numKeys));
            }
            switch (max)
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.Cyan;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Magenta;
                case 4:
                    return Color.Red;
                case 5:
                    return Color.Yellow;
                default:
                    return Color.Blue;
            }
        }

        /// <summary>
        /// Finds the edge tile next to a specified tile.
        /// </summary>
        /// <param name="a"> A point which should be next to the edge. </param>
        /// <returns> Edge tile. </returns>
        private Point FindEdgeTile(Point a)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (tiles.OnEdgeOfTileField(a.X + direction.ToPoint().X, a.Y + direction.ToPoint().Y))
                {
                    return new Point(a.X + direction.ToPoint().X, a.Y + direction.ToPoint().Y);
                }
            }

            // The default end location in case of an error.
            return new Point(0, 1);
        }

        /// <returns>The TileField generated by the generator.</returns>
        private TileField Finish()
        {
            return tiles;
        }
    }
}
