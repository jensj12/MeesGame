using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AI
{
    class FloodFill : IAI
    {
        private Dictionary<KeyColor, int> keys = new Dictionary<KeyColor, int>()
            {
                { KeyColor.KeyBlue, 1},
                { KeyColor.KeyCyan, 2},
                { KeyColor.KeyGreen, 4},
                { KeyColor.KeyMagenta, 8},
                { KeyColor.KeyRed, 16},
                { KeyColor.KeyYellow, 32}
            };

        private Dictionary<Direction, int> slides = new Dictionary<Direction, int>()
            {
                { Direction.NORTH, 0},
                { Direction.EAST, 1},
                { Direction.SOUTH, 2},
                { Direction.WEST, 3}
            };

        IAIPlayer player;
        private Stack<Point> route;
        private bool exitFound;
        private bool[,,,] visited;
        private List<PlayerAction> path;
        private TileField tileField;
        private Point slidingDirection;

        public FloodFill()
        {
        }

        public void GameStart(IAIPlayer player, int difficulty)
        {
            // Initiate path, route and slidingDirection.
            path = new List<PlayerAction>();
            route = new Stack<Point>();
            slidingDirection = new Point();

            this.player = player;

            // Assign the tileField, and create the visited array with the right dimensions.
            tileField = this.player.DummyPlayer.TileField;
            visited = new bool[tileField.Columns, tileField.Rows, 128, 4];

            // Start the FloodFill algorithm.
            Flood(player.DummyPlayer.Location, 0, 0);

            // Turn the route into a list of actions for UpdateNextAction().
            RouteToPath(route);
        }

        public void ThinkAboutNextAction()
        {
        }

        public void UpdateNextAction()
        {
            if (path.Count > 0)
            {
                // Follow the path while there is one.
                player.NextAIAction = path[0];
                path.RemoveAt(0);
            }
            else
            {
                // For if the algorithm can't find the exit.
                player.NextAIAction = PlayerAction.NONE;
            }
        }

        /// <summary>
        /// The main method of the FloodFill AI.
        /// </summary>
        /// <param name="p"> Point to start/continue the flooding from. </param>
        private void Flood(Point p, int keyIndex, int slidingIndex)
        {
            // If the exit has already been found or we have already visited this tile, stop the flooding.
            if (exitFound || visited[p.X, p.Y, keyIndex, slidingIndex])
                return;

            // GuardTile is subclass of HoleTile so this will return for both.
            // Can't use the same for walls because DoorTile is a subclass of WallTile.
            // Can't use doors for which you don't have the key.
            if (tileField.GetTile(p.X, p.Y) is HoleTile ||
                tileField.GetType(p.X, p.Y) == TileType.Wall ||
                (tileField.GetType(p.X, p.Y) == TileType.Door && (keyIndex & keys[(tileField.GetTile(p.X, p.Y) as DoorTile).DoorColor]) == 0))
                return;

            // The current tile has been visited.
            visited[p.X, p.Y, keyIndex, slidingIndex] = true;

            // Add the current tile to the route.
            route.Push(p);

            // If this is the exit, the algorithm is done. 
            // Make sure this is after pushing p to route, otherwise you will stand still before the exit.
            if (tileField.GetTile(p.X, p.Y).TileType == TileType.End)
            {
                exitFound = true;
                return;
            }

            // Check if the current tile is a keytile,
            // and whether we have already picked up a key of this color.
            if (tileField.GetTile(p.X, p.Y).TileType == TileType.Key)
            {
                // Add the key to the keyIndex
                keyIndex |= keys[(tileField.GetTile(p.X, p.Y) as KeyTile).KeyColor];
            }

            // If we're on an ice tile, we need the sliding direction to see if we can change direction.
            if (tileField.GetType(p.X, p.Y) == TileType.Ice)
            {
                // Retrieve the current and previous point in the route.
                Point current = route.Pop();
                Point previous = route.Pop();

                // Calculate the direction you're moving at.
                slidingDirection = current - previous;

                // Return the current and previous point to the route.
                route.Push(previous);
                route.Push(current);
            }

            //If you can move onto the tile behind the ice tile, you can't change direction
            if (tileField.GetType(p.X, p.Y) == TileType.Ice &&
                !(tileField.GetTile(p.X + slidingDirection.X, p.Y + slidingDirection.Y) is WallTile))
            {
                //Only keep going in this direction
                Flood(p + slidingDirection, keyIndex, slides[GetDirectionFromPoint(slidingDirection)]);
            }
            // Otherwise we can move in all directions.
            else
            {
                foreach (PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
                {
                    if (action == PlayerAction.SPECIAL && tileField.GetTile(p.X, p.Y).TileType == TileType.Portal)
                        Flood((tileField.GetTile(p) as PortalTile).Destination, keyIndex, 0);
                    else if (action.IsDirection())
                    {
                        // Obstacles are checked earlier in Flood() so we only need to check whether the new point is in the tilefield.
                        if (!tileField.OutOfTileField(p + action.ToDirection().ToPoint()))
                        {
                            Flood(p + action.ToDirection().ToPoint(), keyIndex, 0);
                        }
                    }
                }
            }

            // All directions are done, return to the previous point.
            // If the exit has been found, keep the route intact.
            if (!exitFound && route.Count > 0)
                route.Pop();
        }

        /// <summary>
        /// Converts a route to a path.
        /// </summary>
        /// <param name="route"> the route to be converted to a path</param>
        private void RouteToPath(Stack<Point> route)
        {
            // route.Count will be 0 if the Flood algorithm can not find the exit.
            if (route.Count == 0) return;
            Point end = route.Pop();
            while (route.Count > 0)
            {
                Point start = route.Pop();
                path.Insert(0, GetActionFromPoints(start, end));
                end = start;
            }
        }

        /// <summary>
        /// Returns the action needed to get from start to end.
        /// </summary>
        /// <param name="start"> starting point </param>
        /// <param name="end"> ending point </param>
        /// <returns> The action needed to get from start to end. </returns>
        private PlayerAction GetActionFromPoints(Point start, Point end)
        {
            if (start + Direction.NORTH.ToPoint() == end)
                return PlayerAction.NORTH;
            if (start + Direction.EAST.ToPoint() == end)
                return PlayerAction.EAST;
            if (start + Direction.SOUTH.ToPoint() == end)
                return PlayerAction.SOUTH;
            if (start + Direction.WEST.ToPoint() == end)
                return PlayerAction.WEST;

            return PlayerAction.SPECIAL;
        }

        private Direction GetDirectionFromPoint(Point p)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                if (p == direction.ToPoint())
                    return direction;

            // Default for error message.
            return Direction.NORTH;
        }
    }
}
