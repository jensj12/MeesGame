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

            SpriteSheetButton begin = new SpriteSheetButton(new DirectionLocation(10, 10, true, false), null, Strings.begin, (UIComponent o) =>
            {
                ((LoadMenuState)GameEnvironment.GameStateManager.GetGameState("LoadMenuState")).UpdateLevelExplorer();
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
            });

            SpriteSheetButton settings = new SpriteSheetButton(new CombinationLocation(new CenteredLocation(horizontalCenter: true), new DirectionLocation(yOffset: 10, topToBottom: false)), null, Strings.settings, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.PreviousGameState = GameEnvironment.GameStateManager.CurrentGameState.ToString();
                GameEnvironment.GameStateManager.SwitchTo("SettingsMenuState");
            });

            SpriteSheetButton quit = new SpriteSheetButton(new DirectionLocation(10, 10, false, false), null, Strings.exit, (UIComponent o) =>
            {
                GameEnvironment.Instance.Exit();
            });

            MenuContainer.AddChild(begin);

            //Settings button
            MenuContainer.AddChild(settings);

            MenuContainer.AddChild(quit);
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
