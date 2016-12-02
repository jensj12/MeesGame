using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MeesGame
{
    internal class LoadMenuState : IGameLoopObject
    {
        private FileExplorer levelExplorer;
        private FileExplorer aiExplorer;
        private Button startButton;

        public LoadMenuState(ContentManager content)
        {
            //check if the directory for levels exists and if not create it
            //for now it reads  (and creates) \MeesGame\bin\Windows\x86\Debug\Content\levels
            //the reading does work, you can check by adding a .lvl (left collum) or a .ai (right collum) to the \levels directory. Just make sure it isn't a txt file
            string directory = content.RootDirectory + "/levels";
            DirectoryInfo info = Directory.CreateDirectory(directory);
            levelExplorer = new FileExplorer(content, new Rectangle(100, 100, 500, 500), "lvl", directory);
            aiExplorer = new FileExplorer(content, new Rectangle(700, 100, 500, 500), "ai", directory);
            startButton = new Button(content, Strings.ok, new Vector2(700, 700), (Object o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("PlayingLevelState");
            });
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            levelExplorer.Draw(gameTime, spriteBatch);
            aiExplorer.Draw(gameTime, spriteBatch);
            startButton.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            levelExplorer.HandleInput(inputHelper);
            aiExplorer.HandleInput(inputHelper);
            startButton.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}