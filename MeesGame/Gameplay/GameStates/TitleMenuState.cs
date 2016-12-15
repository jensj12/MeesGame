using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame;

namespace MeesGame
{
    internal class TitleMenuState : IGameLoopObject
    {
        private UIObjectList<UIObject> gameObjectList;

        public TitleMenuState()
        {
            gameObjectList = new UIObjectList<UIObject>();
            gameObjectList.Add(new Button(new Vector2(10, 10), null, Strings.begin, (UIObject o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("LoadMenuState");
            }));
            gameObjectList.Add(new Button(new Vector2(10, 120), null, Strings.map_editor, (UIObject o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("EditorState");
            }));
            gameObjectList.Add(new Button(new Vector2(10, 230), null, Strings.exit, (UIObject o) =>
            {
                GameEnvironment.GetGameEnvironment.Exit();
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