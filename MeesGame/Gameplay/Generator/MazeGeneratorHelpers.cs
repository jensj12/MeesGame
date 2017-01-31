using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MeesGen
{
    partial class MazeGenerator
    {
        enum behindDoorFlags
        {
            behindBlue = 1,
            behindCyan = 2,
            behindGreen = 4,
            behindMagenta = 8,
            behindRed = 16,
            behindYellow = 32
        }

        /// <summary>
        /// Returns the behindDoorFlags from the specified color.
        /// </summary>
        /// <param name="color"> color to be turned into a flag </param>
        private behindDoorFlags GetFlagFromColor(Color color)
        {
            // Color can't be used in a switch, so use if instead
            if (color == Color.Blue) return behindDoorFlags.behindBlue;
            if (color == Color.Cyan) return behindDoorFlags.behindCyan;
            if (color == Color.Green) return behindDoorFlags.behindGreen;
            if (color == Color.Magenta) return behindDoorFlags.behindMagenta;
            if (color == Color.Red) return behindDoorFlags.behindRed;
            if (color == Color.Yellow) return behindDoorFlags.behindYellow;

            // Default
            return behindDoorFlags.behindBlue;
        }

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
        /// Create an array filled with 0-n in a random order.
        /// </summary>
        /// <returns>Array containing 0 to n in a random order.</returns>
        private static int[] getZeroToNInRandomOrder(int n)
        {
            int[] ints = new int[n];
            for (int i = 0; i < n; i++)
            {
                ints[i] = -1;
            }
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    int pos = random.Next(n);
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

        /// <summary>
        /// Determines the position to expand the maze from: continue the path or from a random position.
        /// </summary>
        private void DetermineNextPositionToExpandFrom()
        {
            position = nodesToDo.Count - 1;
            if (randomNext || randomChance(splitChance))
            {
                // Swap the last node with another node
                int nodeToDoNext = random.Next(nodesToDo.Count);
                swap(nodesToDo, nodeToDoNext, position);
            }
        }

        /// <summary>
        /// Determines the position to place the exit of the portal, make sure it aligns with the rest of the maze.
        /// </summary>
        /// <returns> position to place the second portal </returns>
        private Point DetermineSecondPortalPosition()
        {
            return new Point(random.Next(tiles.Columns / 2 - 1) * 2 + 1, random.Next(tiles.Rows / 2 - 1) * 2 + 1);
        }

        /// <summary>
        /// Chooses a color for the key/door to be placed.
        /// </summary>
        /// <param name="max"> Maximum int to make sure there are no doors placed for which there is no key. </param>
        /// <param name="randomly"> In case of a door, whether the color of the door may be chosen randomly. </param>
        /// <returns> Color of the door/key </returns>
        private Color ChooseColor(int max, bool randomly = false, bool applyKeyColorOrder = true)
        {
            if (randomly)
                max = random.Next(max);
            if (applyKeyColorOrder)
                max = keyColorOrder[max];

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

        /// <summary>
        /// Updates the score of next according to the current score + extra score, preserving highest.
        /// </summary>
        /// <param name="current">Base tile</param>
        /// <param name="addedScore">Score added to that of the base tile</param>
        /// <param name="next">Tile that has its score updated</param>
        private void UpdateBestExitScore(Point current, int addedScore, Point next)
        {
            exitScore[next.X, next.Y] = MathHelper.Max(exitScore[next.X, next.Y], exitScore[current.X, current.Y] + addedScore);
        }

        /// <summary>
        /// Checks if the new exit point is better than the current exit.
        /// </summary>
        private void UpdateBestExit(Point newPoint)
        {
            // The best exit tile has to be next to the edge of the tilefield, the exit will be placed on the edge next to it.
            if (!tiles.NearEdgeOfTileField(newPoint.X, newPoint.Y)) return;

            // The best exit is the one hardest to reach from the start.
            // Start with the distance from the start tile.
            int score = exitScore[newPoint.X, newPoint.Y];
            behindDoorFlags primaryFlags = behindDoorInfo[newPoint.X, newPoint.Y];
            behindDoorFlags secondaryFlags = primaryFlags;

            // For each door the exit is hidden behind, add the doors the key is hidden behind
            foreach (behindDoorFlags flag in Enum.GetValues(typeof(behindDoorFlags)))
            {
                if ((flag & primaryFlags) != 0)
                    secondaryFlags |= keyBehindDoorInfo[(int)Math.Log((int)flag, 2)];
            }

            // Add some points for each required key.
            foreach (behindDoorFlags flag in Enum.GetValues(typeof(behindDoorFlags)))
            {
                if ((flag & secondaryFlags) != 0)
                    score += doorScore;
            }

            // Update the best exit if it's better.
            if (score > bestExitScore)
            {
                bestExit = newPoint;
                bestExitScore = score;
            }
        }

        /// <summary>
        /// The point between two given points
        /// </summary>
        private Point PointBetween(Point one, Point two)
        {
            return new Point((one.X + two.X) / 2, (one.Y + two.Y) / 2);
        }

        /// <summary>
        /// Returns true with the given chance
        /// </summary>
        private bool randomChance(double chance)
        {
            return random.Next(BIGINT) < chance * BIGINT;
        }

        /// <returns>The TileField generated by the generator.</returns>
        private TileField Finish()
        {
            return tiles;
        }
    }
}
