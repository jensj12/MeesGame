using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MeesGame.CharacterAction;


namespace MeesGame.Gameplay.GameObjects
{
    class QLearningAI : Character
    {

        /// <summary>
        /// beginState is the state in which the agent begins a maze.
        /// </summary>
        private Level beginState;

        /// <summary>
        /// discountFactor is a number that determines how much more important earlier rewards are. 
        /// The lower the discountFactor is, the more important it is to the Q function to get rewards as soon as possible.
        /// </summary>
        private double learningRate, discountFactor;

        /// <summary>
        /// qValues is the library in which the Q function assigns the rewards it gets to the states it visits.
        /// Everytime the Q function receives a Level and a PlayerAction, it updates qValues to the new value.
        /// </summary>
        private Dictionary<Tuple<Level, MeesGame.CharacterAction>, double> qValues;

        public QLearningAI(Level level, TileField tileField, Point location, double learningRate, double discountFactor, Dictionary<Tuple<Level, MeesGame.CharacterAction>, double> qValues, int layer = 0, string id = "", int score = 0) : base(level, tileField, location, layer, id, score)
        {
            this.score = score;
            this.level = level;
            this.location = location;
            this.position = tileField.GetAnchorPosition(location);

            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.qValues = qValues;

            //beginState is nodig elke keer dat er opnieuw wordt begonnen en wanneer het leren klaar is
            this.beginState = level;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.Q))
            {
                AIStartTrainingMode();
            }
            else if (inputHelper.IsKeyDown(Keys.S))
            {
                //stop met leren, ook het level 'resetten'
            }
            else if (inputHelper.IsKeyDown(Keys.G))
            {
                //doe het voor het echie, misschien learning rate op 1 zetten zoat hij neit gekke dingen gaat doen
            }
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
        /// It receives an Level s and semi-randomly (depending on the learningRate) chooses an PlayerAction a to perform.
        /// It performs the action and calls the Q function to update the qValues entry for (s, a). 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="learningRate"></param>
        public void AITrainingPerformAction(Level s)
        {

        }
        
        /// <summary>
        /// AIDecideAction receives ILevelStates, determines which action it wants to perform and then performs this action.
        /// </summary>
        //waarom performt deze methode de action al als er een methode AITrainingPerformAction is?
        public void AIDecideAction()
        {

        }

        /// <summary>
        /// Returns the score of the performed action a on the Level s.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private int GetResultOfAction(Level s, MeesGame.CharacterAction a)
        {
            return 0;
        }

        /// <summary>
        /// This function estimates the best qValue that can be obtained by performing an action in the new Level s' that is reached by performing PlayerAction a in Level s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private double EstimatedOptimalFutureValue(Level s, MeesGame.CharacterAction a)
        {
            return 0;
        }

        /// <summary>
        /// The Q function receives a Level s and a to be performed PlayerAction a. 
        /// It determines what the result is of this action and then updates the entry for (s, a) in the qValues library.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="a"></param>
        private void Q(Level s, MeesGame.CharacterAction a)
        {
            Tuple<Level, MeesGame.CharacterAction> StateAndAction = new Tuple<Level, MeesGame.CharacterAction>(s, a);
            qValues[StateAndAction] = qValues[StateAndAction] + learningRate * ((double)GetResultOfAction(s, a) + discountFactor * EstimatedOptimalFutureValue(s, a) - qValues[StateAndAction]);
        }
    }
}
