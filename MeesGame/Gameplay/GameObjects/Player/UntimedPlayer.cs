using Microsoft.Xna.Framework;

namespace MeesGame
{
    class UntimedPlayer : Character
    {
        public UntimedPlayer(TileField tileField, Point location, int score = 0) : base(tileField, location, score)
        {
            OnMoveSmoothly += delegate (Character player, Direction direction) { EndMoveSmoothly(); };
        }
    }
}
