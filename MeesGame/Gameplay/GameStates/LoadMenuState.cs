using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    internal class LoadMenuState : IGameLoopObject
    {
        //we need to use a container, because only elements in a container can eat the input of the other elements
        private UIContainer uiContainer;

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

            //we don't need to inser a valid size because we don't hide the overflow. I can't give a valid one yet because I can't get the correct dimensions of the window
            uiContainer = new UIContainer(Vector2.Zero, Vector2.Zero, null);
            levelExplorer = new FileExplorer(new Vector2(100, 100), new Vector2(500, 500), uiContainer, "lvl", directory);
            aiExplorer = new FileExplorer(new Vector2(700, 100), new Vector2(500, 500), uiContainer, "ai", directory);
            startButton = new Button(new Vector2(700, 700), Vector2.Zero, uiContainer, Strings.ok, (Button o) =>
            {
                GameEnvironment.GameStateManager.SwitchTo("PlayingLevelState");
            });

            uiContainer.AddChild(levelExplorer);
            uiContainer.AddChild(aiExplorer);
            uiContainer.AddChild(startButton);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            uiContainer.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            uiContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            uiContainer.Update(gameTime);
        }
    }
}