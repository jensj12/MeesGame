using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        GameEnvironment game;
        private UIObjectList<UIObject> gameObjectList;
        private ImageList imageList;
        private 

        public LevelEditorState(GameEnvironment game)
        {
            this.game = game;
            gameObjectList = new UIObjectList<UIObject>();
            //I have to make 825 a constant variable because the spritebatch has this size but I can't find any
            //variable pointing to this size.....
            imageList = new ImageList(new Vector2(0, 0), new Vector2(200, 825), null, OnItemSelect);
            gameObjectList.Add(imageList);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameObjectList.Draw(gameTime, spriteBatch);
        }

        public void OnItemSelect(Object o)
        {

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