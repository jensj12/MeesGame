using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MeesGen
{
    class MazeGenerator
    {
        private const int BIGINT = 100000;

        /// <summary>
        /// Generates a random maze using backtracking. Use odd dimensions for best effect.
        /// </summary>
        /// <param name="tiles">TileField to be populated</param>
        /// <param name="startX">Starting position of the algorithm</param>
        /// <param name="startY">Starting position of the algorithm</param>
        /// <param name="loopChance">Chance that existing routes will not block new routes, thus creating a loop</param>
        /// <param name="splitChance">Chance that the algorithm will randomly continue at another point, low values generate longer paths, but with less junctions</param>
        /// <returns>The populated TileField</returns>
        public static MeesGame.TileField GenerateMaze(MeesGame.TileField tiles, int startX, int startY, double loopChance = 0.01, double splitChance = 0.03)
        {
            for (int y = 0; y < tiles.Columns; y++)
            {
                for (int x = 0; x < tiles.Rows; x++)
                {
                    tiles.Add(new MeesGame.WallTile(), x, y);
                }
            }

            Random random = GameEnvironment.Random;
            IList<Point> nodesToDo = new List<Point>();
            Point start = new Point(startX, startY);
            if (startX < 0 || startY < 0 || startX >= tiles.Columns || startY >= tiles.Rows)
            {
                start = new Point(random.Next(tiles.Columns / 2) * 2, random.Next(tiles.Rows / 2) * 2);
            }
            nodesToDo.Add(start);
            tiles.Add(new MeesGame.FloorTile(0, "playerstart"), start.X, start.Y);

            bool randomNext = false;
            while (nodesToDo.Count != 0)
            {
                int position = nodesToDo.Count - 1;
                if (randomNext || random.Next(BIGINT) < splitChance * BIGINT)
                {
                    int nodeToDoNext = random.Next(nodesToDo.Count);
                    swap(nodesToDo, nodeToDoNext, position);
                }
                Point current = nodesToDo[position];
                Point next;
                int[] possible = { 0, 0, 0, 0 };
                for (int i = 1; i < 5; i++)
                {
                    while (true)
                    {
                        int pos = random.Next(4);
                        if (possible[pos] != 0) continue;
                        possible[pos] = i;
                        break;
                    }
                } //possible now contains 1-4 in random order

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
                    if (!(tiles.GetTile(next) is MeesGame.WallTile))
                    {
                        if (random.Next(BIGINT) < loopChance * BIGINT)
                            tiles.Add(new MeesGame.FloorTile(), (next.X + current.X) / 2, (next.Y + current.Y) / 2);
                        continue;
                    }

                    nodesToDo.Add(next);
                    tiles.Add(new MeesGame.FloorTile(), next.X, next.Y);
                    tiles.Add(new MeesGame.FloorTile(), (next.X + current.X) / 2, (next.Y + current.Y) / 2);
                    randomNext = false;
                    break;
                }
                if (randomNext)
                {
                    nodesToDo.RemoveAt(position);
                }
            }

            return tiles;
        }

        private static void swap(IList<Point> list, int pos1, int pos2)
        {
            Point save = list[pos1];
            list[pos1] = list[pos2];
            list[pos2] = save;
        }
    }
}
