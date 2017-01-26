using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private UIComponent MenuContainer;

        public TitleMenuState()
        {
            MenuContainer = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            MenuContainer.AddChild(new Background(new SpriteSheet("mainMenuOverlay").Sprite));

            MenuContainer.AddChild(new SpriteSheetButton(new SimpleLocation(10, 715), null, Strings.begin, (UIComponent o) =>
            {
                ((LoadMenuState)GameEnvironment.GameStateManager.GetGameState("LoadMenuState")).UpdateLevelExplorer();
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
            }));
            MenuContainer.AddChild(new SpriteSheetButton(new SimpleLocation(500, 715), null, Strings.map_editor, (UIComponent o) =>
            {
                //reset the gamestate to open blank;
                GameEnvironment.GameStateManager.GetGameState("EditorState").Reset();
                GameEnvironment.GameStateManager.SwitchTo("EditorState");
            }));
            MenuContainer.AddChild(new SpriteSheetButton(new SimpleLocation(1000, 715), null, Strings.exit, (UIComponent o) =>
            {
                GameEnvironment.Instance.Exit();
            }));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MenuContainer.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            MenuContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
            MenuContainer.Reset();
        }

        public void Update(GameTime gameTime)
        {
            MenuContainer.Update(gameTime);
        }
    }
}
