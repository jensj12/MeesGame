using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        ContentManager content;
        List<PlayingLevel> level;
        int currentLevelIndex;

        public PlayingLevelState(ContentManager content)
        {
            this.content = content;
            level = new List<PlayingLevel>();
            level.Add(new PlayingLevel());
            currentLevelIndex = 0;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level[currentLevelIndex].HandleInput(inputHelper);
        }

        public void Update(GameTime gameTime)
        {
            level[currentLevelIndex].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level[currentLevelIndex].Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
            level[currentLevelIndex].Reset();
        }
    }
}
