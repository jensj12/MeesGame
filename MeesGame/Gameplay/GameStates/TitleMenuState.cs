using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private UIComponent MenuContainer;

        public TitleMenuState()
        {
            MenuContainer = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            MenuContainer.AddChild(new Background(new SpriteSheet("mainMenuOverlay").Sprite));

            List<UIComponent> buttons = new List<UIComponent>();

            CombinationLocation buttonLocation = new CombinationLocation(new SharedLocation(buttons, -40, 0), new DirectionLocation(yOffset: 10, topToBottom: false));

            SpriteSheetButton begin = new SpriteSheetButton(buttonLocation, null, Strings.begin, (UIComponent o) =>
            {
                ((LoadMenuState)GameEnvironment.GameStateManager.GetGameState("LoadMenuState")).UpdateLevelExplorer();
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
            });

            SpriteSheetButton createLevel = new SpriteSheetButton(buttonLocation, null, Strings.map_editor, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LevelEditorState");
                //reset the gamestate to open blank;		
                GameEnvironment.GameStateManager.CurrentGameState.Reset();
            });

            SpriteSheetButton settings = new SpriteSheetButton(buttonLocation, null, Strings.settings, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.PreviousGameState = GameEnvironment.GameStateManager.CurrentGameState.ToString();
                GameEnvironment.GameStateManager.SwitchTo("SettingsMenuState");
            });

            SpriteSheetButton quit = new SpriteSheetButton(buttonLocation, null, Strings.exit, (UIComponent o) =>
            {
                GameEnvironment.Instance.Exit();
            });

            buttons.Add(begin);
            buttons.Add(createLevel);
            buttons.Add(settings);
            buttons.Add(quit);


            //Level Editor button;
            MenuContainer.AddChild(begin);

            MenuContainer.AddChild(createLevel);

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
