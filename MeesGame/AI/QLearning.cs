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
        private List<bool> keysInInventory; 

        public AILevelState(Point location, List<bool> keysInInventory)
        {
            this.location = location;
            this.keysInInventory = keysInInventory;
        }
    }

    class QLearning : GameObject
    {
        /// <summary>
        /// beginState is the state in which the agent begins a maze.
        /// </summary>
        private AILevelState beginState;


        private Level level;

        private Player player;

        /// <summary>
        /// currentState is the state in which the agent currently is.
        /// </summary>
        private AILevelState currentState;

        /// <summary>
        /// learningRate is a number that determines how important it is to try out new ways of completing the maze. 
        /// If it is very high, it will choose a random action more often. If it is very low, it will often stick to the known best action.
        /// 
        /// discountFactor is a number that determines how much more important earlier rewards are.
        /// The lower discountFactor is, the more important it is to the Q function to get rewards as soon as possible.
        /// </summary>
        private double learningRate, discountFactor;

        /// <summary>
        /// This list contains bools that show wether you have a certain key or not. 
        /// currentKeyList[i] == 1 if you have the key corresponding with index i, it is 0 if you don't have that key.
        /// </summary>
        private List<bool> currentKeyList;

        /// <summary>
        /// qValues is the library in which the Q function assigns the rewards it gets to the states it visits.
        /// Everytime the Q function receives an ILevelState and a PlayerAction, it updates qValues to the new value.
        /// </summary>
        private Dictionary<Tuple<AILevelState, PlayerAction>, double> qValues;

        private Dictionary<AILevelState, PlayerAction> bestAction;
        
        //er moet nog iets zijn wat bijhoudt welke keys zijn opgepakt om te weten in welke qValue je moet zijn

        public QLearning(Level level, double learningRate, double discountFactor) {
            this.level = level;

            //TODO this.currentKeyList = currentKeyList;
            this.currentState = new AILevelState(player.Location, currentKeyList);

            //voor als we verschillende opties doen; als we altijd op dezelfde manier beginnen kunnen we dit ook weglaten uit de constructor en gewoon een waarde geven.
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;

            //beginState is nodig elke keer dat er opnieuw wordt begonnen en wanneer het leren klaar is
            //this.beginState = level;
        }
        
        public override void HandleInput(InputHelper inputHelper)
        {
            Thread train = new Thread(AIStartTrainingMode);

            if (inputHelper.IsKeyDown(Keys.Q))
            {
                level.timeBetweenActions = TimeSpan.FromMilliseconds(5);

                train.Start();
                train.IsBackground = true;
            }
            else if (inputHelper.IsKeyDown(Keys.S))
            {
                //stop met leren, ook het level 'resetten'
                train.Abort();
                //level = beginState;
            }
            else if (inputHelper.IsKeyDown(Keys.G))
            {
                //doe het voor het echie, learning rate op 1 zetten zoat hij neit gekke dingen gaat doen
                learningRate = 1;

                level.timeBetweenActions = TimeSpan.FromMilliseconds(300);
            }
        }

        public int numberOfKeys()
        {
            int numKeys = 0;

            for (int i = 0; i < level.NumRows; i++)
            {
                for (int j = 0; j < level.NumColumns; j++)
                {
                    if (level.Tiles.GetType(i, j) == TileType.Key)
                    {
                        numKeys++;
                    }
                }
            }
            return numKeys;
        }

        /// <summary>
        /// This function starts the training mode.
        /// </summary>
        public void AIStartTrainingMode()
        {
            while (true)
            {
                while (level.Tiles.GetType(player.Location.X,player.Location.Y) != TileType.End)
                {
                    AITrainingModeDoMove(currentState);
                }
            }
        }

        /// <summary>
        /// This function picks an action to perform during the trainins phase.
        /// It receives an ILevelState s and semi-randomly (depending on the learningRate) chooses an PlayerAction a to perform.
        /// It performs the action and calls the Q function to update the qValues entry for (s, a).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="learningRate"></param>
        public void AITrainingModeDoMove(AILevelState s)
        {
            double r1 = ((double) GameEnvironment.Random.Next()) / 2147483646;
            
            //als het random getal r1 (tussen 0 en 1) kleiner is dan learningRate, dan kiest hij de beste actie voor deze positie
            if (r1 <= learningRate && bestAction.ContainsKey(s))
            {
                currentState = GetNewAILevelState(bestAction[s]);
                player.PerformAction(bestAction[s]);
            }

            //zo niet, dan kiest hij een random actie
            else // misschien moet er nog iets gebeuren als er geen mogelijke acties zijn
            {
                int r2 = GameEnvironment.Random.Next();

                List<PlayerAction> possibleActions = new List<PlayerAction>();

                foreach (PlayerAction action in player.Actions)
                {
                    currentState = GetNewAILevelState(bestAction[s]);
                    possibleActions.Add(action);
                }

                player.PerformAction(possibleActions[r2 % possibleActions.Count]);
            }
            //zorg dat bestActions geupdatet wordt
            //zorg dat de currentKeyList wordt geupdatet
        }

        /// <summary>
        /// AIDecideAction receives AILevelStates, determines which action it wants to perform and then performs this action.
        /// </summary>
        //waarom performt deze methode de action al als er een methode AITrainingPerformAction is? Omdat dit níét voor de trainingmodus is maar voor de echte modus.
        //Oké misschien heb je gelijk, misschien moet deze methode weg.
        public void AIPerformAction()
        {
            //zorg dat de currentKeyList wordt geupdatet
        }

        /// <summary>
        /// Returns the score of the performed action a on the Level s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private int GetResultOfAction(Level s, PlayerAction a)
        {
            return 0;
        }

        /// <summary>
        /// Returns the AILevelState you reach by performing PlayerAction a at AILevelState s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private AILevelState GetNewAILevelState(PlayerAction a)
        {
            List<bool> temporaryKeyList = new List<bool>();

            foreach (bool element in currentKeyList)
            {
                temporaryKeyList.Add(element); //zorg dat de tijdelijke lijst geupdatet wordt als je op een vakje met een sleutel komt
            }
            Player clone = player.Clone();
            clone.PerformAction(a);
            AILevelState newAILevelState = new AILevelState(clone.Location, temporaryKeyList);

            return newAILevelState;
        }

        /// <summary>
        /// This function estimates the best qValue that can be obtained by performing an action in the new Level s' that is reached by performing PlayerAction a in Level s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private double EstimatedOptimalFutureValue(AILevelState s, PlayerAction a)
        {
            Tuple<AILevelState, PlayerAction> northKey = getKey(GetNewAILevelState(a), PlayerAction.NORTH);
            Tuple<AILevelState, PlayerAction> eastKey = getKey(GetNewAILevelState(a), PlayerAction.EAST);
            Tuple<AILevelState, PlayerAction> westKey = getKey(GetNewAILevelState(a), PlayerAction.WEST);
            Tuple<AILevelState, PlayerAction> southKey = getKey(GetNewAILevelState(a), PlayerAction.SOUTH);
            Tuple<AILevelState, PlayerAction> noneKey = getKey(GetNewAILevelState(a), PlayerAction.NONE);

            double northValue = qValues[northKey];
            double eastValue = qValues[eastKey];
            double westValue = qValues[westKey];
            double southValue = qValues[southKey];
            double noneValue = qValues[noneKey];

            double[] array = { northValue, eastValue, westValue, southValue, noneValue };

            return array.Max();
        }

        /// <summary>
        /// Given a AILevelState s and a PlayerAction a, this returns a usable key for the qValues dictionary.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Tuple<AILevelState, PlayerAction> getKey(AILevelState s, PlayerAction a)
        {
            Tuple<AILevelState, PlayerAction> newKey = new Tuple<AILevelState, PlayerAction>(s, a);
            return newKey;
        }

        /// <summary>
        /// AIPlayGame receives ILevelStates, determines which action it wants to perform and then performs this action.
        /// </summary>
        public void AIPlayGame()
        {
            player = new TimedPlayer(level,level.Start);
            level.UsePlayer(player);
        }

        /// <summary>
        /// Returns the score of the performed action a on the ILevelSTate s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private int GetResultOfAction(AILevelState s, PlayerAction a)
        {
            return 0;
        }

        /// <summary>
        /// The Q function receives a ILevelState s and a to be performed PlayerAction a.
        /// It determines what the result is of this action and then updates the entry for (s, a) in the qValues library.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private void Q(AILevelState s, PlayerAction a)
        {
            Tuple<AILevelState, PlayerAction> StateAndAction = new Tuple<AILevelState, PlayerAction>(s, a);
            qValues[StateAndAction] = qValues[StateAndAction] + learningRate * ((double)GetResultOfAction(s, a) + discountFactor * EstimatedOptimalFutureValue(s, a) - qValues[StateAndAction]);
        }
    }
}