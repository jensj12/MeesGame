using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MeesGame
{
    internal class LoadMenuState : IGameLoopObject
    {
        //we need to use a container, because only elements in a container can eat the input of the other elements
        private UIComponent menuContainer;

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
            menuContainer = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
            levelExplorer = new FileExplorer(new SimpleLocation(100, 100), new SimpleDimensions(500, 500), "lvl", directory);
            aiExplorer = new FileExplorer(new SimpleLocation(700, 100), new SimpleDimensions(500, 500), "ai", directory);
            startButton = new SpriteSheetButton(new CenteredLocation(0, 700, true, false), null, Strings.generate_random_maze, (UIComponent o) =>
            {
                GameEnvironment.GameStateManager.PreviousGameState = "LoadMenuState";
                PlayingLevelState state = (PlayingLevelState)GameEnvironment.GameStateManager.GetGameState("PlayingLevelState");
                TileField tileField;
                if (levelExplorer.SelectedFile != null)
                {
                    tileField = FileIO.Load(levelExplorer.SelectedFile);
                }
                else
                {
                    tileField = MeesGen.MazeGenerator.GenerateMaze();
                }
                state.StartLevel(new PlayingLevel(tileField));
                GameEnvironment.GameStateManager.GetGameState("PlayingLevelState").Reset();
                GameEnvironment.GameStateManager.SwitchTo("PlayingLevelState");
            });
            menuContainer.AddChild(levelExplorer);
            menuContainer.AddChild(aiExplorer);
            menuContainer.AddChild(startButton);

            levelExplorer.FileSelected += OnLevelSelect;
        }

        public void UpdateFileExplorers()
        {
            levelExplorer.generateFileList();
            aiExplorer.generateFileList();
        }

        public void OnLevelSelect(FileExplorer fileExplorer)
        {
            startButton.Text = Strings.loadLevel;
            foreach (Button button in levelExplorer.Children)
            {
                if (button.Selected)
                {
                    startButton.Text = Strings.loadLevel;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menuContainer.Draw(gameTime, spriteBatch);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            menuContainer.HandleInput(inputHelper);
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            menuContainer.Update(gameTime);
        }
    }
}
