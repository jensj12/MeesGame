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
        void GameStart(MeesGame.IPlayer player, int difficulty);

        /// <summary>
        /// Whenever the AI player needs a new action, this function is called.
        /// </summary>
        /// <returns>Next action to perform by the AI player</returns>
        MeesGame.PlayerAction GetNextAction();
    }
}