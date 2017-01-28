using AI;
using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace MeesGame
{
    class AIPlayer : PlayerGameObject, IAIPlayer
    {
        readonly TimeSpan MAX_UPDATE_NEXT_ACTION_TIME = TimeSpan.FromMilliseconds(50);
        IAI AI;
        Thread thinkNextActionThread;
        bool startingThinkNextActionThread = false;

        public AIPlayer(IAI AI, Level level, Point location, int score = 0) : base(level, location, score)
        {
            this.AI = AI;
            AI.GameStart(this, 3);
            startNextActionThreadAsync();
            OnPlayerAction += delegate (PlayerGameObject obj)
            {
                startNextActionThreadAsync();
            };
        }

        private void startNextActionThreadAsync()
        {
            new Thread(startNextActionThread).Start();
        }

        private void startNextActionThread()
        {
            // to prevent this function from being called at the same time by multiple threads
            if (!startingThinkNextActionThread)
            {
                startingThinkNextActionThread = true;

                //let the currently running thread finish
                thinkNextActionThread?.Join();

                //call ThinkAbountNextAction in a new thread
                thinkNextActionThread = new Thread(AI.ThinkAboutNextAction);
                thinkNextActionThread.Start();

                startingThinkNextActionThread = false;
            }
        }

        /// <summary>
        /// The action that will be performed next, set by the AI
        /// </summary>
        public PlayerAction NextAIAction
        {
            private get; set;
        }

        /// <summary>
        /// The action that will be performed next
        /// </summary>
        protected override PlayerAction NextAction
        {
            get
            {
                if (!IsMoving)
                {
                    Thread updateNextActionThread = new Thread(AI.UpdateNextAction);
                    updateNextActionThread.Start();
                    updateNextActionThread.Join(MAX_UPDATE_NEXT_ACTION_TIME);
                }
                return NextAIAction;
            }
        }
    }
}
