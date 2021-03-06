﻿namespace MeesGame
{
    interface IAIPlayer
    {
        /// <summary>
        /// The Level the player is playing on.
        /// </summary>
        Level Level
        {
            get;
        }

        /// <summary>
        /// The TileField the player is playing on.
        /// </summary>
        TileField TileField
        {
            get;
        }

        IPlayer DummyPlayer
        {
            get;
        }

        /// <summary>
        /// The next action that should be performed according to the AI
        /// </summary>
        PlayerAction NextAIAction
        {
            set;
        }
    }
}
