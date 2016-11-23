using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        
        protected override void LoadContent()
        {
            base.LoadContent();
            screen = new Point(1440, 825);
            windowSize = new Point(1024, 586);
            FullScreen = false;
            gameStateManager.AddGameState("PlayingLevelState", new PlayingLevelState(Content));
            /* uncomment to add gamestates
            gameStateManager.AddGameState("titleMenu", new TitleMenuState());
            gameStateManager.AddGameState("helpState", new HelpState());
            gameStateManager.AddGameState("playingState", new PlayingState(Content));
            gameStateManager.AddGameState("levelMenu", new LevelMenuState());
            gameStateManager.AddGameState("gameOverState", new GameOverState());
            gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
            gameStateManager.SwitchTo("titleMenu");//*/
        }
    }
}
