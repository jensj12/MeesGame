using Microsoft.Xna.Framework;

namespace MeesGame
{
    /// <summary>
    /// A character that moves instantly. That is, EndMoveSmoothly() is called immediately after MoveSmoothly
    /// </summary>
    class UntimedCharacter : Character
    {
        public UntimedCharacter(TileField tileField, Point location, int score = 0) : base(tileField, location, score)
        {
            OnMoveSmoothly += delegate (Character player, Direction direction) { EndMoveSmoothly(); };
        }
    }
}
