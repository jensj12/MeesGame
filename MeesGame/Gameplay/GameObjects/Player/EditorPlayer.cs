using Microsoft.Xna.Framework;
using static MeesGame.PlayerAction;
using Microsoft.Xna.Framework.Input;

namespace MeesGame
{
    /// <summary>
    /// A player that can move over obstacles
    /// </summary>
    class EditorPlayer : AnimatedMovingGameObject
    {
        PlayerAction action = NONE;
        public delegate void EditorPlayerEvent(EditorPlayer player);
        public event EditorPlayerEvent OnMove;

        public EditorPlayer(Level level, Point location) : base(level.Tiles, level.TimeBetweenActions, "player@4x4", 0, "", 0)
        {
            Teleport(location);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsMoving && action != NONE)
            {
                if (!field.OutOfTileField(Location + action.ToDirection().ToPoint()))
                    MoveSmoothly(action.ToDirection());
                OnMove?.Invoke(this);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.W) || inputHelper.IsKeyDown(Keys.Up))
            {
                action = NORTH;
            }
            else if (inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right))
            {
                action = EAST;
            }
            else if (inputHelper.IsKeyDown(Keys.S) || inputHelper.IsKeyDown(Keys.Down))
            {
                action = SOUTH;
            }
            else if (inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left))
            {
                action = WEST;
            }
            else
            {
                action = NONE;
            }
        }
    }
}
