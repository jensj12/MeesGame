using Microsoft.Xna.Framework;

namespace MeesGame
{
    class UntimedPlayer : DummyPlayer
    {
        public UntimedPlayer(TileField tileField, Point location, int score = 0) : base(tileField, location, score)
        {
            OnMoveSmoothly += delegate (DummyPlayer player, Direction direction) { EndMoveSmoothly(); };
        }
    }
}
