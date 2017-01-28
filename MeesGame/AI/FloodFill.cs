using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AI
{
    class FloodNode //  ---
    {
        private Point point;
        private int keyIndex;
        private int slidingIndex;

        public FloodNode(Point point, int keyIndex, int slidingIndex)
        {
            this.point = point;
            this.keyIndex = keyIndex;
            this.slidingIndex = slidingIndex;
        }

        public Point Point
        {
            get
            {
                return point;
            }
        }

        public int KeyIndex
        {
            get
            {
                return keyIndex;
            }

            set
            {
                keyIndex |= value;
            }
        }

        public int SlidingIndex
        {
            get
            {
                return slidingIndex;
            }
        }

    }

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
        private Stack<FloodNode> nodes; //  ---
        //private Stack<Point> route;
        //private bool exitFound;
        private bool[,,,] visited;
        private List<PlayerAction> path;
        private TileField tileField;
        private Point slidingDirection;

        public FloodFill()
        {
        }

        public void GameStart(IAIPlayer player, int difficulty)
        {
            // Initiate path, nodes, route and slidingDirection.
            path = new List<PlayerAction>();
            nodes = new Stack<FloodNode>(); //  ---
            //route = new Stack<Point>();
            slidingDirection = new Point();

            this.player = player;

            // Assign the tileField, and create the visited array with the right dimensions.
            tileField = this.player.DummyPlayer.TileField;
            visited = new bool[tileField.Columns, tileField.Rows, 128, 4];

            nodes.Push(new FloodNode(player.DummyPlayer.Location, 0, 0));   //  ---

            // Start the FloodFill algorithm.
            //Flood();    //  ---

            // Turn the route into a list of actions for UpdateNextAction().
            RouteToPath(Flood());
        }

        public void ThinkAboutNextAction()
        {
        }

        public void UpdateNextAction()
        {
            if (path.Count > 0)
            {
                // Follow the path while there is one.
                player.NextAIAction = path[path.Count - 1];
                path.RemoveAt(path.Count - 1);
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
  //      /// <param name="p"> Point to start/continue the flooding from. </param>
        private Stack<FloodNode> Flood()    //  ---
        {
            while (nodes.Count > 0) //  ---
            {
                FloodNode node = nodes.Pop();
                
                // If the exit has already been found or we have already visited this tile, stop the flooding.
                if (visited[node.Point.X, node.Point.Y, node.KeyIndex, node.SlidingIndex])
                    continue;

                // GuardTile is subclass of HoleTile so this will return for both.
                // Can't use the same for walls because DoorTile is a subclass of WallTile.
                // Can't use doors for which you don't have the key.
                if (tileField.GetTile(node.Point.X, node.Point.Y) is HoleTile ||
                    tileField.GetType(node.Point.X, node.Point.Y) == TileType.Wall ||
                    (tileField.GetType(node.Point.X, node.Point.Y) == TileType.Door && (node.KeyIndex & keys[(tileField.GetTile(node.Point.X, node.Point.Y) as DoorTile).DoorColor]) == 0))
                    continue;

                // The current tile has been visited.
                visited[node.Point.X, node.Point.Y, node.KeyIndex, node.SlidingIndex] = true;

                // Add the current tile to the route.
                //route.Push(node.Point);

                // If this is the exit, the algorithm is done. 
                // Make sure this is after pushing p to route, otherwise you will stand still before the exit.
                if (tileField.GetTile(node.Point.X, node.Point.Y).TileType == TileType.End)
                {
                    //exitFound = true;
                    return nodes;
                }

                // Check if the current tile is a keytile,
                // and whether we have already picked up a key of this color.
                if (tileField.GetTile(node.Point.X, node.Point.Y).TileType == TileType.Key)
                {
                    // Add the key to the keyIndex
                    node.KeyIndex = keys[(tileField.GetTile(node.Point.X, node.Point.Y) as KeyTile).KeyColor];
                }

                // If we're on an ice tile, we need the sliding direction to see if we can change direction.
                if (tileField.GetType(node.Point.X, node.Point.Y) == TileType.Ice)
                {
                    // Retrieve the current and previous point in the route.
                    FloodNode current = nodes.Pop();
                    FloodNode previous = nodes.Pop();

                    // Calculate the direction you're moving at.
                    slidingDirection = current.Point - previous.Point;

                    // Return the current and previous point to the route.
                    nodes.Push(previous);
                    nodes.Push(current);
                }

                //If you can move onto the tile behind the ice tile, you can't change direction
                if (tileField.GetType(node.Point.X, node.Point.Y) == TileType.Ice &&
                    !(tileField.GetTile(node.Point.X + slidingDirection.X, node.Point.Y + slidingDirection.Y) is WallTile))
                {
                    //Only keep going in this direction
                    nodes.Push(new FloodNode(node.Point + slidingDirection, node.KeyIndex, slides[GetDirectionFromPoint(slidingDirection)]));
                    continue;
                }
                // Otherwise we can move in all directions.
                else
                {
                    foreach (PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
                    {
                        if (action == PlayerAction.SPECIAL && tileField.GetTile(node.Point.X, node.Point.Y).TileType == TileType.Portal)
                        {
                            nodes.Push(new FloodNode((tileField.GetTile(node.Point) as PortalTile).Destination, node.KeyIndex, 0));
                            continue;
                        }
                        else if (action.IsDirection())
                        {
                            // Obstacles are checked earlier in Flood() so we only need to check whether the new point is in the tilefield.
                            if (!tileField.OutOfTileField(node.Point + action.ToDirection().ToPoint()))
                            {
                                //Flood(p + action.ToDirection().ToPoint(), keyIndex, 0);   ---
                                nodes.Push(new FloodNode(node.Point + action.ToDirection().ToPoint(), node.KeyIndex, 0));
                                continue;
                            }
                        }
                    }
                }

                // All directions are done, return to the previous point.
                // If the exit has been found, keep the route intact.
                //if (!exitFound && route.Count > 0)
                //    route.Pop();

                nodes.Pop();
            }

            return nodes;
        }

        /// <summary>
        /// Converts a route to a path.
        /// </summary>
        /// <param name="route"> the route to be converted to a path</param>
        private void RouteToPath(Stack<FloodNode> route)
        {
            // route.Count will be 0 if the Flood algorithm can not find the exit.
            if (route.Count == 0) return;
            Point end = route.Pop().Point;
            while (route.Count > 0)
            {
                Point start = route.Pop().Point;
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
