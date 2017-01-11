using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;

namespace MeesGame
{
    public class BasicMeesGame : GameEnvironment
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BasicMeesGame())
                game.Run();
        }

        public BasicMeesGame()
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            screen = new Point(1440, 825);
            windowSize = new Point(1024, 586);
            FullScreen = false;
            gameStateManager.AddGameState("PlayingLevelState", new PlayingLevelState());
            gameStateManager.AddGameState("TitleMenuState", new TitleMenuState());
            gameStateManager.AddGameState("EditorState", new LevelEditorState());
            gameStateManager.AddGameState("LoadMenuState", new LoadMenuState());
            gameStateManager.AddGameState("GameOverState", new GameOverState());
            gameStateManager.SwitchTo("TitleMenuState");

            assetManager.PlayMusic("theme_song");
        }
    }
}
