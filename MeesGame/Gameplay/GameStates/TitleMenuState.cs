using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private UIContainer UIContainer;

        public TitleMenuState()
        {
            UIContainer = new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());
            UIContainer.AddChild(new Button(new Vector2(10, 10), null, Strings.begin, (UIObject o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
            }));
            UIContainer.AddChild(new Button(new Vector2(10, 120), null, Strings.map_editor, (UIObject o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("EditorState");
            }));
            UIContainer.AddChild(new Button(new Vector2(10, 230), null, Strings.exit, (UIObject o) =>
            {
                GameEnvironment.Instance.Exit();
            }));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            UIContainer.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            UIContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            UIContainer.Update(gameTime);
        }
    }
}