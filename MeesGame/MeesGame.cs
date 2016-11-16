using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class MeesGame : GameEnvironment
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MeesGame())
                game.Run();
        }

        public MeesGame()
        {
            Content.RootDirectory = "Content";
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();
            screen = new Point(1440, 825);
            windowSize = new Point(1024, 586);
            FullScreen = false;
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
