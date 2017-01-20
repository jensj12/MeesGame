using MeesGame;

namespace AI
{
    /// <summary>
    /// This interface defines all functions the AI player will ever call.
    /// Any AI should implement these and should not depend on any other function calls.
    /// This should make it easy to connect the AI to the game and make sure the AI won't do anything else.
    /// The AI is allowed to make dummy players and move them around.
    /// </summary>
    interface IAI
    {
        /// <summary>
        /// Only called at game start.
        /// </summary>
        /// <param name="player">The player the AI will take control of.</param>
        /// <param name="difficulty">The difficulty level (1-5) of the AI</param>
        void GameStart(IAIPlayer player, int difficulty);

        /// <summary>
        /// Whenever the AI player needs a new action, this function is called. 
        /// N.B. This function is called async to ThinkAboutNextAction. ThinkAboutNextAction might still be running while this function is called.
        /// This function should update player.NextAIAction. After this function finishes, the specified action will be performed by the IAIPlayer 
        /// This function should not take more time than AIPlayer.MAX_UPDATE_NEXT_ACTION_TIME or it will be ignored.
        /// </summary>
        void UpdateNextAction();

        /// <summary>
        /// Called whenever the NextAction has been performed and a new action is required.
        /// Can do some work so that it does not need to be done in UpdateNextAction
        /// </summary>
        void ThinkAboutNextAction();
    }
}
