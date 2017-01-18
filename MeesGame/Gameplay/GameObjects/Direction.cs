using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }

    static class DirectionExtensions
    {
        /// <summary>
        /// Converts the direction into a point with absolute value 1
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>A point with absolute value 1 in the direction relative to the origin</returns>
        public static Point ToPoint(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return new Point(0, -1);
                case Direction.EAST:
                    return new Point(1, 0);
                case Direction.SOUTH:
                    return new Point(0, 1);
                case Direction.WEST:
                    return new Point(-1, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts the direction into a vector with absolute value 1
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>A vector with absolute value 1 in the direction relative to the origin</returns>
        public static Vector2 ToVector2(this Direction direction)
        {
            Point p = direction.ToPoint();
            return new Vector2(p.X, p.Y);
        }

        public static bool IsDirection(this PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.NORTH:
                case PlayerAction.EAST:
                case PlayerAction.SOUTH:
                case PlayerAction.WEST:
                    return true;
                default:
                    return false;
            }
        }

        public static Direction ToDirection(this PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.NORTH:
                    return Direction.NORTH;
                case PlayerAction.EAST:
                    return Direction.EAST;
                case PlayerAction.SOUTH:
                    return Direction.SOUTH;
                case PlayerAction.WEST:
                    return Direction.WEST;
                default:
                    throw new PlayerActionNotAllowedException();
            }
        }

        public static int ToSheetIndex(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return 0;
                case Direction.EAST:
                    return 1;
                case Direction.SOUTH:
                    return 2;
                case Direction.WEST:
                    return 3;
            }
            throw new NotImplementedException();
        }
    }
}
