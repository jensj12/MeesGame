using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeesGame
{
    class GameOverState : IGameLoopObject
    {
        public UIContainer overlay = new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());
        public GameOverState()
        {
            overlay.AddChild(new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2(), Color.Black));
            overlay.AddChild(new Button(new Vector2(600, 400), null, Strings.victory, (UIObject o) =>
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
        //TODO: Add a draw function once the overlay is here
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
