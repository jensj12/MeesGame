using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeesGame
{
    internal class LoadMenuState : IGameLoopObject
    {
        private const int settingsDistance = 20;

        public static readonly Dictionary<int, string> PlayerTypes = new Dictionary<int, string>()
        {
            {0, "Human" },
            {1, "Random" },
            {2, "A*" },
            {3, "MonteCarlo" }
        };

        public static readonly Dictionary<int, string> difficultyLevels = new Dictionary<int, string>()
        {
            {0, "Mega Easy" },
            {1, "Easy" },
            {2, "Medium" },
            {3, "Hard" },
            {4, "Legendary" }
        };

        //we need to use a container, because only elements in a container can eat the input of the other elements
        private UIComponent menuContainer;

        private FileExplorer levelExplorer;
        private SortedList levelSettings;

        UIComponent difficultyBox;

        List<RadioButton> playerRadioGroup;
        List<RadioButton> difficultyRadioGroup;


        private Button startButton;
        private Button randomButton;

        public LoadMenuState()
        {
            //check if the directory for levels exists and if not create it
            //for now it reads  (and creates) \MeesGame\bin\Windows\x86\Debug\Content\levels
            //the reading does work, you can check by adding a .lvl (left column) or a .ai (right column) to the \levels directory. Just make sure it isn't a txt file
            string directory = GameEnvironment.AssetManager.Content.RootDirectory + "\\levels";
            DirectoryInfo info = Directory.CreateDirectory(directory);

            menuContainer = new UIComponent(SimpleLocation.Zero, null);

            UIComponent centerContainer = new UIComponent(new CenteredLocation(0, 100, true), WrapperDimensions.All);
            levelExplorer = new FileExplorer(SimpleLocation.Zero, new SimpleDimensions(500, 500), "lvl", directory);
            levelSettings = new SortedList(new RelativeToLocation(levelExplorer, 100, relativeToLeft: false), new SimpleDimensions(500, 500), settingsDistance * 2);
            levelSettings.AddConstantComponent(new Background(Utility.SolidWhiteTexture, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.FileExplorerBackground)));

            startButton = new SpriteSheetButton(new CenteredLocation(0, 700, true, false), null, Strings.begin, (UIComponent o) =>
            {
                BeginLevel();
            });

            centerContainer.AddChild(levelExplorer);
            centerContainer.AddChild(levelSettings);
            menuContainer.AddChild(centerContainer);
            menuContainer.AddChild(startButton);

            InitializeLevelSettingsMenu();
        }

        private void BeginLevel()
        {
            PlayingLevelState state = (PlayingLevelState)GameEnvironment.GameStateManager.GetGameState("PlayingLevelState");
            TileField tileField;
            if (levelExplorer.SelectedFile == randomButton)
            {
                tileField = MeesGen.MazeGenerator.GenerateMaze(difficulty: DifficultyIndex() + 1);
            }
            else
            {
                try
                {
                    tileField = FileIO.Load(levelExplorer.GetPathFromChild(levelExplorer.SelectedFile));
                }
                catch (Exception)
                {
                    return;
                }
            }

            GameEnvironment.GameStateManager.PreviousGameState = "LoadMenuState";
            state.StartLevel(new PlayingLevel(tileField, 0, PlayerIndex()));
            GameEnvironment.GameStateManager.GetGameState("PlayingLevelState").Reset();
            GameEnvironment.GameStateManager.SwitchTo("PlayingLevelState");
        }

        /// <summary>
        /// Fills the levelsettings box.
        /// </summary>
        private void InitializeLevelSettingsMenu()
        {
            UIComponent playerSelectBox = new UIComponent(SimpleLocation.Zero, WrapperDimensions.All);

            playerRadioGroup = new List<RadioButton>();

            Textbox playerSelectText = new Textbox(SimpleLocation.Zero, null, "Player", textColor: Color.Black);

            playerSelectBox.AddChild(playerSelectText);

            for (int i = 0; i < PlayerTypes.Count; i++)
            {
                RadioButton radioBotton = new RadioButton(new RelativeToLocation(((i == 0) ? playerSelectText : (UIComponent)playerRadioGroup[i - 1]), (i == 0) ? settingsDistance : 0, settingsDistance, relativeToTop: false), PlayerTypes[i], playerRadioGroup);
                if (i == 0)
                    radioBotton.Selected = true;
                playerSelectBox.AddChild(radioBotton);
            }

            levelSettings.AddChild(playerSelectBox);

            difficultyBox = new UIComponent(SimpleLocation.Zero, WrapperDimensions.All);

            difficultyRadioGroup = new List<RadioButton>();

            Textbox difficutlyText = new Textbox(SimpleLocation.Zero, null, "Maze Difficulty", textColor: Color.Black);

            difficultyBox.AddChild(difficutlyText);

            for (int i = 0; i < difficultyLevels.Count; i++)
            {
                RadioButton radioBotton = new RadioButton(new RelativeToLocation((i == 0) ? difficutlyText : (UIComponent)difficultyRadioGroup[i - 1], (i == 0) ? settingsDistance : 0, settingsDistance, relativeToTop: false), difficultyLevels[i], difficultyRadioGroup);
                if(i == 2)
                    radioBotton.Selected = true;
                difficultyBox.AddChild(radioBotton);
            }

            levelSettings.AddChild(difficultyBox);
        }

        public void UpdateLevelExplorer()
        {
            levelExplorer.Reset();
            randomButton = new SpriteSheetButton(Vector2.Zero, null, "Random maze");
            randomButton.SelectedChanged += (UIComponent component) =>
            {
                if (((Button)component).Selected)
                {
                    if(!levelSettings.Children.Contains(difficultyBox))
                        levelSettings.AddChild(difficultyBox);
                }
                else if (levelSettings.Children.Contains(difficultyBox))
                {
                    levelSettings.Children.Remove(difficultyBox);
                    levelSettings.Invalidate();
                }
            };
            levelExplorer.ChildClickHandler(randomButton);

            randomButton.Selected = true;     

            levelExplorer.AddChild(randomButton);
            levelExplorer.GenerateFileList();
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
            menuContainer.Reset();
        }

        public void Update(GameTime gameTime)
        {
            menuContainer.Update(gameTime);
        }

        /// <summary>
        /// Index of the selected playertype.
        /// </summary>
        /// <returns></returns>
        private int PlayerIndex()
        {
            int playerIndex;
            for (playerIndex = 0; playerIndex < playerRadioGroup.Count; playerIndex++)
            {
                if ((playerRadioGroup[playerIndex]).Selected)
                    break;
            }
            return playerIndex;
        }

        /// <summary>
        /// Index of the selected Difficulty.
        /// </summary>
        /// <returns></returns>
        private int DifficultyIndex()
        {
            int difficultyIndex;
            for (difficultyIndex = 0; difficultyIndex < difficultyBox.Children.Count; difficultyIndex++)
            {
                if ((difficultyRadioGroup[difficultyIndex]).Selected)
                    break;
            }
            return difficultyIndex;
        }
    }
}
