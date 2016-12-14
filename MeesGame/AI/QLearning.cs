using System;
using System.Collections.Generic;

namespace AI
{
    // Tijdelijk type om errors te voorkomen, moet GUIteindelijk ergens in de MeesGame namespace worden gedefiniëerd.
    class ILevelState
    {

    }

    class QLearning
    {
        /// <summary>
        /// beginState is the state in which the agent begins a maze.
        /// </summary>
        private ILevelState beginState;

        /// <summary>
        /// discountFactor is a number that determines how much more important earlier rewards are. 
        /// The lower discountFactor is, the more important it is to the Q function to get rewards as soon as possible.
        /// </summary>
        private double learningRate, discountFactor;

        /// <summary>
        /// qValues is the library in which the Q function assigns the rewards it gets to the states it visits.
        /// Everytime the Q function receives an ILevelState and a PlayerAction, it updates qValues to the new value.
        /// </summary>
        private Dictionary<Tuple<ILevelState, MeesGame.PlayerAction>, double> qValues;

        public QLearning(double learningRate, double discountFactor, Dictionary<Tuple<ILevelState, MeesGame.PlayerAction>, double> qValues)
        {
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.qValues = qValues;
        }

        /// <summary>
        /// This function starts the training mode. 
        /// </summary>
        public void AIStartTrainingMode()
        {
            // iets van een thread (als hij een end state bereikt moet hij opnieuw gaan)
        }

        /// <summary>
        /// This function picks an action to perform during the trainins phase. 
        /// It receives an ILevelState s and semi-randomly (depending on the learningRate) chooses an PlayerAction a to perform.
        /// It performs the action and calls the Q function to update the qValues entry for (s, a). 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="learningRate"></param>
        public void AITrainingModeDoMove(ILevelState s)
        {

        }

        /// <summary>
        /// AIPlayGame receives ILevelStates, determines which action it wants to perform and then performs this action.
        /// </summary>
        public void AIPlayGame()
        {

        }

        /// <summary>
        /// Returns the score of the performed action a on the ILevelSTate s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private int GetResultOfAction(ILevelState s, MeesGame.PlayerAction a)
        {
            return 0;
        }

        /// <summary>
        /// This function estimates the best qValue that can be obtained by performing an action in the new ILevelState s' that is reached by performing PlayerAction a in ILevelState s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private double EstimatedOptimalFutureValue(ILevelState s, MeesGame.PlayerAction a)
        {
            return 0;
        }

        /// <summary>
        /// The Q function receives a ILevelState s and a to be performed PlayerAction a. 
        /// It determines what the result is of this action and then updates the entry for (s, a) in the qValues library.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private void Q(ILevelState s, MeesGame.PlayerAction a)
        {
            Tuple<ILevelState, MeesGame.PlayerAction> StateAndAction = new Tuple<ILevelState, MeesGame.PlayerAction>(s, a);
            qValues[StateAndAction] = qValues[StateAndAction] + learningRate * ((double)GetResultOfAction(s, a) + discountFactor * EstimatedOptimalFutureValue(s, a) - qValues[StateAndAction]);
        }
    }
}
