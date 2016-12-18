using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeesGame.Gameplay.GameStates
{
    class GameOverState : IGameLoopObject
    {
        public GameOverState()
        {

        }

        //Upon pressing space, return to the menu/editor
        //TODO: Make the user go back to the editor if he came from there
        public void HandleInput(InputHelper inputHelper)
        {
            if (!inputHelper.KeyPressed(Keys.Space))
                return;
            else
                GameEnvironment.GameStateManager.SwitchTo(GameEnvironment.GameStateManager.PreviousGameState);
        }

        //No need to update anything, it's game over
        public void Update(GameTime gameTime)
        {
            return;
        }

        //Draw the correct gameover overlay
        //TODO: Add a draw function once the overlay is here
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        //Switching to a different state already resets everything
        public void Reset()
        {
            return;
        }
    }
}
