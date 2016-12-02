using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        GameEnvironment game;
        private UIContainer gameObjectList;
        private UIList imageList;

        public LevelEditorState(GameEnvironment game)
        {
            this.game = game;
            gameObjectList = new UIContainer(Vector2.Zero, Vector2.Zero, null);
            //I have to make 825 a constant variable because the spritebatch has this size but I can't find any
            //variable pointing to this size.....
            imageList = new UIList(new Vector2(0, 0), new Vector2(200, 825), null);
            imageList.onItemClick += OnItemSelect;
            gameObjectList.AddChild(imageList);
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