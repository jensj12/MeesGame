using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeesGame
{
    class GameOverState : IGameLoopObject
    {
        public UIComponent Overlay { get; set; } = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
        public GameOverState()
        {
            Overlay.AddChild(new UIComponent(SimpleLocation.Zero, InheritDimensions.All));
        }

        public void HandleInput(InputHelper inputHelper)
        {
            //Upon pressing space, return to the menu/editor
            if (inputHelper.KeyPressed(Keys.Space))
                GameEnvironment.GameStateManager.SwitchTo(GameEnvironment.GameStateManager.PreviousGameState);

            Overlay.HandleInput(inputHelper);
        }

        public void Update(GameTime gameTime)
        {
            Overlay.Update(gameTime);
        }

        //Draw the correct gameover overlay
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Overlay.Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
        }
    }
}
