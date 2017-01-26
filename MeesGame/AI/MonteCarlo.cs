using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static MeesGame.PlayerAction;
using NodeActionPair = System.Collections.Generic.KeyValuePair<MeesGame.PlayerAction, AI.Node>;

namespace AI
{
    class MonteCarlo : IAI
    {
        public static int SCOREMULTIPLIER = 100;

        private Node entryPoint;
        private MeesGame.IAIPlayer AIplayer;
        private MeesGame.IPlayer player;
        private Point startLocation;
        private bool[,] visited;
        private Point gridSize;
        private Node[,] nodeGrid;

        /// <summary>
        /// At the end of the treesearch, the nodes in the nodestack will have their score updated.
        /// </summary>
        private Stack<Node> nodeStack;

        // The following fields define the strength of the AI
        public int MAXITERATIONS = 20;
        public int BUILDTREEPERACTION = 2;
        //private bool keepTrackOfWhatIveDone = true;

        /// <summary>
        /// Empty, useless constructor
        /// </summary>
        public MonteCarlo()
        {
        }

        /// <summary>
        /// Initialises and resets the AI.
        /// </summary>
        public void GameStart(MeesGame.IAIPlayer player, int difficulty = 4)
        {
            difficulty = MathHelper.Clamp(difficulty, 1, 5);
            switch (difficulty)
            {
                case 1:
                    MAXITERATIONS = 5;
                    BUILDTREEPERACTION = 1;
                    break;
                case 2:
                    MAXITERATIONS = 5;
                    break;
                case 3:
                    MAXITERATIONS = 10;
                    BUILDTREEPERACTION = 3;
                    break;
                case 4:
                    MAXITERATIONS = 15;
                    BUILDTREEPERACTION = 5;
                    break;
                case 5:
                    // can be quite slow
                    MAXITERATIONS = 25;
                    BUILDTREEPERACTION = 6;
                    break;
            }
            this.AIplayer = player;
            startLocation = AIplayer.DummyPlayer.Location;
            gridSize = new Point(player.TileField.Columns, player.TileField.Rows);
            Reset();
        }

        private void Reset()
        {
            entryPoint = new Node(AIplayer.DummyPlayer.Location);
            visited = new bool[gridSize.X, gridSize.Y];
            nodeGrid = new Node[gridSize.X, gridSize.Y];
        }

        /// <summary>
        /// Make the AI pick the next action.
        /// </summary>
        public void UpdateNextAction()
        {
            player = AIplayer.DummyPlayer;

            visited[player.Location.X, player.Location.Y] = true;

            MeesGame.PlayerAction action = NONE;
            foreach (NodeActionPair NAP in entryPoint.next)
            {
                if (NAP.Value.maxScore > entryPoint.maxScore)
                {
                    action = NAP.Key;
                    entryPoint = NAP.Value;
                    if (!AIplayer.DummyPlayer.PossibleActions.Contains(action)) breakpoint();
                    break;
                }
            }
            AIplayer.NextAIAction = action;
            return;
        }

        public void ThinkAboutNextAction()
        {
            player = AIplayer.DummyPlayer;

            visited[player.Location.X, player.Location.Y] = true;

            BuildTree(BUILDTREEPERACTION);
        }

        /// <summary>
        /// Helper method to get the reverse direction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private MeesGame.PlayerAction GetReverseAction(MeesGame.PlayerAction action)
        {
            switch (action)
            {
                case NORTH:
                    return SOUTH;
                case EAST:
                    return WEST;
                case SOUTH:
                    return NORTH;
                case WEST:
                    return EAST;
                default:
                    return NONE;
            }
        }

        /// <summary>
        /// Helper method that calls BuildTree(), might be slow if high values are used.
        /// </summary>
        /// <param name="n">Number of times to call BuildTree.</param>
        public void BuildTree(int n)
        {
            for (int i = 0; i < n; i++)
            {
                BuildTree();
            }
        }

        /// <summary>
        /// This function should be called at least once before checking the best possible move.
        /// </summary>
        public void BuildTree()
        {
            MeesGame.IPlayer dummy = player.Clone();
            Point curLoc = dummy.Location;
            int iterations = 0;
            Node curNode = entryPoint;
            nodeStack = new Stack<Node>();
            nodeStack.Push(curNode);

            while (iterations < MAXITERATIONS)
            {
                iterations += 1;
                // pick a random action to perform
                MeesGame.PlayerAction chosenAction = dummy.PossibleActions[GameEnvironment.Random.Next(dummy.PossibleActions.Count)];
                if (chosenAction == NONE/* || (GetReverseAction(chosenAction) == lastAction)*/) continue;

                //perform action
                dummy.PerformAction(chosenAction);
                nodeStack.Push(curNode);
                curNode = goToNextNode(curNode, chosenAction, dummy.Location);
                curLoc = dummy.Location;
                if (!visited[curLoc.X, curLoc.Y])
                {
                    curNode.maxScore = getScore(curLoc);
                    break;
                }
            }

            //max number of iterations reached, or broken out of the loop
            updateScore();
        }

        /// <summary>
        /// Selects the node the player is on after performing the action that brings him to the given location.
        /// </summary>
        /// <param name="curNode">The current node.</param>
        /// <param name="usedAction">The action performed.</param>
        /// <param name="nextLocation">The location after performing the action.</param>
        /// <returns></returns>
        private Node goToNextNode(Node curNode, MeesGame.PlayerAction usedAction, Point nextLocation)
        {
            // Check if the node already has the next node in the list
            foreach (NodeActionPair NAP in curNode.next)
            {
                if (NAP.Key == usedAction)
                {
                    if (NAP.Value.location != nextLocation) breakpoint();
                    return NAP.Value;
                }
            }

            Node node;
            // No node available, create a new one
            if (nodeGrid[nextLocation.X, nextLocation.Y] == null)
            {
                node = new Node(nextLocation);
                nodeGrid[nextLocation.X, nextLocation.Y] = node;
                curNode.next.Add(usedAction, node);
                return node;
            }

            // The node is in the grid, use it and add it as neighbour
            node = nodeGrid[nextLocation.X, nextLocation.Y];
            curNode.next.Add(usedAction, node);
            return node;
        }

        /// <summary>
        /// Picks the correct score for a not yet visisted tile
        /// </summary>
        private int getScore(Point location)
        {
            MeesGame.Tile tile = player.TileField.GetTile(location);
            switch (tile.Data.TileType)
            {
                case MeesGame.TileType.Floor:
                case MeesGame.TileType.Ice:
                    return SCOREMULTIPLIER;
                case MeesGame.TileType.Door:
                    return 2 * SCOREMULTIPLIER;
                case MeesGame.TileType.Key:
                    return 3 * SCOREMULTIPLIER;
                case MeesGame.TileType.End:
                    return 10 * SCOREMULTIPLIER;
                case MeesGame.TileType.Hole:
                case MeesGame.TileType.Guard:
                    return -SCOREMULTIPLIER;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Invalidates every node in the nodeStack
        /// </summary>
        private void updateScore()
        {
            while (nodeStack.Count >= 1)
            {
                Node node = nodeStack.Pop();
                node.Invalidate(entryPoint);
            }
        }

        private void breakpoint()
        {
            return;
        }
    }

    /// <summary>
    /// Represents a node in the monte carlo search tree.
    /// The class Node is used as a struct, but using a class removes the limitations of a struct.
    /// </summary>
    class Node
    {
        public int maxScore;
        public Point location;
        public IDictionary<MeesGame.PlayerAction, Node> next;

        public Node(Point location)
        {
            this.location = location;
            maxScore = -MonteCarlo.SCOREMULTIPLIER;
            next = new Dictionary<MeesGame.PlayerAction, Node>();
        }

        /// <summary>
        /// Update the score of the node.
        /// </summary>
        public void Invalidate(Node zeroNode)
        {
            maxScore = -10 * MonteCarlo.SCOREMULTIPLIER;
            foreach (NodeActionPair node in next)
            {
                maxScore = MathHelper.Max(maxScore, (node.Value == zeroNode) ? -10 * MonteCarlo.SCOREMULTIPLIER : node.Value.maxScore - 1);
            }
        }
    }
}