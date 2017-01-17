namespace MeesGame
{
    interface IAIPlayer
    {
        /// <summary>
        /// The level the AI is playing on
        /// </summary>
        Level Level
        {
            get;
        }

        /// <summary>
        /// The TileField the character is on
        /// </summary>
        TileField TileField
        {
            get;
        }

        ICharacter Character
        {
            get;
        }
    }
}
