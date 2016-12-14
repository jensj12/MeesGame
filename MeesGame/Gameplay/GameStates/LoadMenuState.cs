﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MeesGame
{
    internal class LoadMenuState : IGameLoopObject
    {
        //we need to use a container, because only elements in a container can eat the input of the other elements
        private GUIContainer GUIContainer;

        private FileExplorer levelExplorer;
        private FileExplorer aiExplorer;
        private Button startButton;

        public LoadMenuState()
        {
            //check if the directory for levels exists and if not create it
            //for now it reads  (and creates) \MeesGame\bin\Windows\x86\Debug\Content\levels
            //the reading does work, you can check by adding a .lvl (left column) or a .ai (right column) to the \levels directory. Just make sure it isn't a txt file
            string directory = GameEnvironment.AssetManager.Content.RootDirectory + "/levels";
            DirectoryInfo info = Directory.CreateDirectory(directory);

            //we don't need to insert a valid size because we don't hide the overflow. I can't give a valid one yet because I can't get the correct dimensions of the window
            GUIContainer = new GUIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());
            levelExplorer = new FileExplorer(new Vector2(100, 100), new Vector2(500, 500), "lvl", directory);
            aiExplorer = new FileExplorer(new Vector2(700, 100), new Vector2(500, 500), "ai", directory);
            startButton = new Button(new Vector2(700, 700), null, Strings.ok, (GUIObject o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("PlayingLevelState");
            });

            GUIContainer.AddChild(levelExplorer);
            GUIContainer.AddChild(aiExplorer);
            GUIContainer.AddChild(startButton);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GUIContainer.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            GUIContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            GUIContainer.Update(gameTime);
        }
    }
}