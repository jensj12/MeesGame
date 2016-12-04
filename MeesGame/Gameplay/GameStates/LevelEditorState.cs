using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;
using System.Collections.Generic;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        const int leftBarWidth = 100;
        const int rightBarWidth = 200;


        GameEnvironment game;
        List<Level> level;
        int currentLevelIndex;

        private UIContainer UIContainer; 
        private UIList imageList;
        private UIList itemPropertiesList;

        public LevelEditorState(GameEnvironment game)
        {
            this.game = game;
            UIContainer = new UIContainer(Vector2.Zero, Vector2.Zero, null);
            imageList = new UIList(new Vector2(0, 0), new Vector2(leftBarWidth, GameEnvironment.Screen.Y), UIContainer);
            itemPropertiesList = new UIList(new Vector2(GameEnvironment.Screen.X - rightBarWidth, 0), new Vector2(rightBarWidth, GameEnvironment.Screen.Y), UIContainer);
            imageList.onItemClick += OnItemSelect;
            UIContainer.AddChild(imageList);
            UIContainer.AddChild(itemPropertiesList);

            level = new List<Level>();
            Level newLevel = new Level(new Point(GameEnvironment.Screen.X - (leftBarWidth + rightBarWidth), GameEnvironment.Screen.Y));
            newLevel.Position += new Vector2(leftBarWidth, 0);
            level.Add(newLevel);
            currentLevelIndex = 0;

            FillImageList();
        }

        private void FillImageList()
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level[currentLevelIndex].Draw(gameTime, spriteBatch);
            UIContainer.Draw(gameTime, spriteBatch);
        }

        public void OnItemSelect(Object o)
        {

            throw new NotImplementedException();
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level[currentLevelIndex].HandleInput(inputHelper);
            UIContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
            level[currentLevelIndex].Reset();
        }

        public void Update(GameTime gameTime)
        {
            level[currentLevelIndex].Update(gameTime);
            UIContainer.Update(gameTime);
        }
    }
}