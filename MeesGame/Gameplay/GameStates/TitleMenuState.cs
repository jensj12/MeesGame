using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private GameObjectList gameObjectList;
        private GameEnvironment game;

        public TitleMenuState(GameEnvironment game)
        {
            this.game = game;
            gameObjectList = new GameObjectList();
            gameObjectList.Add(new Button(game.Content, Strings.begin, new Vector2(10, 10), (Object o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
           }));
            gameObjectList.Add(new Button(game.Content, Strings.map_editor, new Vector2(10, 120), (Object o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("EditorState");
            }));
            gameObjectList.Add(new Button(game.Content, Strings.exit, new Vector2(10, 230), (Object o) =>
            {
                //very crude, but effectively gets the job done ;) (this is actually a paradox).
                game.Exit();
                }));

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameObjectList.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            gameObjectList.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            gameObjectList.Update(gameTime);
        }
    }
}