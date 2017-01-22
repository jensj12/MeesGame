using System;
using System.Collections.Generic;
using MeesGame;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace AI
{
    /// <summary>
    /// Deze class bevat alle informatie over het level die de qLearning AI nodig heeft om te leren.
    /// </summary>
    class AILevelState
    {
        private Point location;
        private ISet<InventoryItem> itemsInInventory; 

        public AILevelState(Point location, ISet<InventoryItem> itemsInInventory)
        {
            this.location = location;
            this.itemsInInventory = itemsInInventory;
        }

        public override int GetHashCode()
        {
            const int START_PRIME = 17;
            const int MULTIPLY_PRIME = 37;

            int result = START_PRIME;
            foreach(InventoryItem item in itemsInInventory)
            {
                result = MULTIPLY_PRIME * result + item.GetHashCode();
            }
            result = MULTIPLY_PRIME * result + location.X;
            result = MULTIPLY_PRIME * result + location.Y;
            return result;
        }

        public override bool Equals(object obj)
        {
            if(obj is AILevelState)
            {
                AILevelState state = (AILevelState)obj;
                return itemsInInventory.SequenceEqual(state.itemsInInventory) && location.Equals(state.location);
            }else
            {
                return false;
            }
        }

        public AILevelState Clone()
        {
            return new AILevelState(location, itemsInInventory);
        }
    }

    class QLearning : IAI
    {
        /// <summary>
        /// beginState is the state in which the agent begins a maze.
        /// </summary>
        private AILevelState beginState;


        private ITileField level;

        private IPlayer player;

        /// <summary>
        /// currentState is the state in which the agent currently is, initialState is the state in which the agent begins the level.
        /// </summary>
        private AILevelState currentState, initialState; // TODO: zorg dat de initialState een waarde krijgt

        /// <summary>
        /// learningRate is a number that determines how important it is to try out new ways of completing the maze. 
        /// If it is very high, it will choose a random action more often. If it is very low, it will often stick to the known best action.
        /// 
        /// discountFactor is a number that determines how much more important earlier rewards are.
        /// The lower discountFactor is, the more important it is to the Q function to get rewards as soon as possible.
        /// </summary>
        private double learningRate, discountFactor;


        IAIPlayer aiPlayer;

        /// <summary>
        /// qValues is the library in which the Q function assigns the rewards it gets to the states it visits.
        /// Everytime the Q function receives an ILevelState and a PlayerAction, it updates qValues to the new value.
        /// </summary>
        private Dictionary<Tuple<AILevelState, PlayerAction>, double> qValues;

        private Dictionary<AILevelState, PlayerAction> bestAction;
        
        //er moet nog iets zijn wat bijhoudt welke keys zijn opgepakt om te weten in welke qValue je moet zijn

        public QLearning(ITileField level, double learningRate, double discountFactor)
        {
            this.level = level;

            //TODO this.currentKeyList = currentKeyList;
            currentState = new AILevelState(player.Location, player.Inventory.Items);

            initialState = new AILevelState(player.Location, player.Inventory.Items);

            //voor als we verschillende opties doen; als we altijd op dezelfde manier beginnen kunnen we dit ook weglaten uit de constructor en gewoon een waarde geven.
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;

            //beginState is nodig elke keer dat er opnieuw wordt begonnen en wanneer het leren klaar is
            //this.beginState = level;
        }
        
        public void GameStart(IPlayer player)
        {
            beginState = new AILevelState(player.Location, player.Inventory.Items); // TODO: zorg dat de keys in inventory klopt
            AIStartTrainingMode();
        }

        public PlayerAction GetNextAction()
        {
            PlayerAction action;
            if (bestAction.ContainsKey(currentState))
            {
                action = bestAction[currentState];
            }
            else
            {
                int r2 = GameEnvironment.Random.Next();

                List<PlayerAction> possibleActions = new List<PlayerAction>();

                foreach (PlayerAction a in player.PossibleActions)
                {
                    possibleActions.Add(a);
                }

                action = possibleActions[r2 % possibleActions.Count];
            }

            currentState = GetNewAILevelState(player, action);

            return action;
        }

        /// <summary>
        /// This function starts the training mode.
        /// </summary>
        public void AIStartTrainingMode()
        {
            for (int x = 0; x < 1000; x++)
            {
                IPlayer clone = player.Clone();
                while (clone.CurrentTile.TileType != TileType.End)
                {
                    AITrainingModeDoMove(clone, currentState);
                }
                currentState = initialState.Clone();
                // TODO: reset de currentState naar de initialState zonder dat de initialState verandert
            }
        }

        /// <summary>
        /// This function picks an action to perform during the training phase.
        /// It receives an ILevelState s and semi-randomly (depending on the learningRate) chooses an PlayerAction a to perform.
        /// It performs the action and calls the Q function to update the qValues entry for (s, a).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="learningRate"></param>
        public void AITrainingModeDoMove(IPlayer clone, AILevelState s)
        {
            double r1 = ((double) GameEnvironment.Random.Next()) / 2147483646;
            
            // If the random number r1 (between 0 and 1) is smaller than learningRate, the best known action for this position is chosen
            if (r1 <= learningRate && bestAction.ContainsKey(s))
            {
                Q(clone, currentState, bestAction[s]);
                currentState = GetNewAILevelState(clone, bestAction[s]);
                clone.PerformAction(bestAction[s]);
            }

            // Otherwise, a random action will be chosen
            else // TODO: misschien moet er nog iets gebeuren als er geen mogelijke acties zijn
            {
                int r2 = GameEnvironment.Random.Next();

                List<PlayerAction> possibleActions = new List<PlayerAction>();

                foreach (PlayerAction action in clone.PossibleActions) 
                {
                    possibleActions.Add(action);
                }
                
                PlayerAction a = possibleActions[r2 % possibleActions.Count];
                Tuple<AILevelState, PlayerAction> StateAndAction = new Tuple<AILevelState, PlayerAction>(s, a);

                Q(clone, currentState, a);
                currentState = GetNewAILevelState(clone, a);
                clone.PerformAction(a);

                // If the randomly chosen action is better than the previous best action, bestAction is updated
                if (bestAction.ContainsKey(s))
                {
                    PlayerAction b = bestAction[s];
                    Tuple<AILevelState, PlayerAction> StateAndBestAction = new Tuple<AILevelState, PlayerAction>(s, b);
                    if (qValues[StateAndAction] > qValues[StateAndBestAction])
                    {
                        bestAction[s] = a;
                    }
                }

                // If there no known best action, the randomly chosen action automatically becomes the new best action.
                else
                {
                    bestAction[s] = a;
                }
            }
            //TODO: zorg dat de currentKeyList wordt geupdatet
        }

        /// <summary>
        /// Returns the AILevelState you reach by performing PlayerAction a at AILevelState s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private AILevelState GetNewAILevelState(IPlayer player, PlayerAction a)
        {
            IPlayer clone = player.Clone();
            clone.PerformAction(a);
            AILevelState newAILevelState = new AILevelState(clone.Location, clone.Inventory.Items);

            return newAILevelState;
        }

        /// <summary>
        /// This function estimates the best qValue that can be obtained by performing an action in the new Level s' that is reached by performing PlayerAction a in Level s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private double EstimatedOptimalFutureValue(IPlayer clone, AILevelState s, PlayerAction a)
        {
            List<double> possibleValues = new List<double>();
            foreach(PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
            {
                Tuple<AILevelState, PlayerAction> actionKey = new Tuple<AILevelState, PlayerAction>(s, a);
                if (qValues.ContainsKey(actionKey))
                {
                    possibleValues.Add(qValues[actionKey]);
                }
            }
            return possibleValues.Max();
        }

        /// <summary>
        /// Returns the score of the performed action a on the ILevelState s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private int GetResultOfAction(IPlayer player, AILevelState s, PlayerAction a)
        {
            IPlayer clone = player.Clone();
            clone.PerformAction(a);

            if (clone.CurrentTile is EndTile) return 1;
            else if (clone.CurrentTile is HoleTile) return -1;
            else return 0; 
        }

        /// <summary>
        /// The Q function receives a ILevelState s and a to be performed PlayerAction a.
        /// It determines what the result is of this action and then updates the entry for (s, a) in the qValues library.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private void Q(IPlayer clone, AILevelState s, PlayerAction a)
        {
            Tuple<AILevelState, PlayerAction> StateAndAction = new Tuple<AILevelState, PlayerAction>(s, a);
            qValues[StateAndAction] = qValues[StateAndAction] + learningRate * ((double)GetResultOfAction(clone, s, a) + discountFactor * EstimatedOptimalFutureValue(clone, s, a) - qValues[StateAndAction]);
        }

        public void GameStart(IPlayer player, int difficulty)
        {
            this.player = player;
        }

        public void UpdateNextAction()
        {
            aiPlayer.NextAIAction = PlayerAction.NONE;
        }

        public void GameStart(IAIPlayer player, int difficulty)
        {
            this.aiPlayer = player;
        }

        public void ThinkAboutNextAction()
        {
            IPlayer player = aiPlayer.DummyPlayer;
            beginState = new AILevelState(player.Location, player.Inventory.Items);
            AIStartTrainingMode();
        }
    }
}