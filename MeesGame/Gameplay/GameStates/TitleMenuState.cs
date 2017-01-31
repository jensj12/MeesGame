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

            //Level Editor button
            MenuContainer.AddChild(new SpriteSheetButton(new CenteredLocation(-54, 715, true), null, Strings.map_editor, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LevelEditorState");
                //reset the gamestate to open blank;
                GameEnvironment.GameStateManager.CurrentGameState.Reset();
            }));

            //Settings button
            MenuContainer.AddChild(new SpriteSheetButton(new CenteredLocation(359, 715, true), null, Strings.settings, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.PreviousGameState = GameEnvironment.GameStateManager.CurrentGameState.ToString();
                GameEnvironment.GameStateManager.SwitchTo("SettingsMenuState");
            }));

            MenuContainer.AddChild(new SpriteSheetButton(new SimpleLocation(1280, 715), null, Strings.exit, (UIComponent o) =>
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
