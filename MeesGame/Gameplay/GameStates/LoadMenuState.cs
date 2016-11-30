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
            levelExplorer = new FileExplorer(new Vector2(100, 100), new Vector2(500, 500), null, content, "lvl", directory);
            aiExplorer = new FileExplorer(new Vector2(700, 100), new Vector2(500, 500), null, content, "ai", directory);
            startButton = new Button(new Vector2(700, 700), Vector2.Zero, null, content, Strings.ok, (Object o) =>
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
            levelExplorer.Update(gameTime);
            aiExplorer.Update(gameTime);
        }
    }
}