using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        ContentManager content;

        public PlayingLevelState(ContentManager content)
        {
            this.content = content;
        }

        public void HandleInput(InputHelper inputHelper)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public void Reset()
        {

        }
    }
}
