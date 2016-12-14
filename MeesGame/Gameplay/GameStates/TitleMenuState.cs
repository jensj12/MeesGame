using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private UIObjectList<UIObject> gameObjectList;
        private GameEnvironment game;

        public TitleMenuState(GameEnvironment game)
        {
            this.game = game;
            gameObjectList = new UIObjectList<UIObject>();
            gameObjectList.Add(new Button(new Vector2(10, 10), new Vector2(100, 100), null, game.Content, Strings.begin, (Button o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
           }));
            gameObjectList.Add(new Button(new Vector2(10, 120), Vector2.Zero, null, game.Content, Strings.map_editor, (Button o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("EditorState");
            }));
            gameObjectList.Add(new Button(new Vector2(10, 230), Vector2.Zero, null, game.Content, Strings.exit, (Button o) =>
            {
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