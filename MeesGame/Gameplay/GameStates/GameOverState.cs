using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeesGame
{
    class GameOverState : IGameLoopObject
    {
        public UIComponent overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
        public GameOverState()
        {
            overlay.AddChild(new UIComponent(SimpleLocation.Zero, InheritDimensions.All));
            overlay.AddChild(new SpriteSheetButton(new SimpleLocation(600, 400), null, Strings.victory, (UIComponent o) =>
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState")));
        }

        //Upon pressing space, return to the menu/editor
        //TODO: Make the user go back to the editor if he came from there
        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Space))
                GameEnvironment.GameStateManager.SwitchTo(GameEnvironment.GameStateManager.PreviousGameState);
            overlay.HandleInput(inputHelper);
        }

        //No need to update anything, it's game over
        public void Update(GameTime gameTime)
        {
            overlay.Update(gameTime);
            return;
        }

        //Draw the correct gameover overlay
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            overlay.Draw(gameTime, spriteBatch);
        }

        //Switching to a different state already resets everything
        public void Reset()
        {
            return;
        }
    }
}
