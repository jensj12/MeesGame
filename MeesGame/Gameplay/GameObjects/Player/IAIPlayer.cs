namespace MeesGame
{
    interface IAIPlayer
    {
        Level Level
        {
            get;
        }

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
