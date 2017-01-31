using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// State of the current game.
/// </summary>
enum PlayingState
{
    Playing, Victory, Defeat
}

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        private const int INVENTORY_WIDTH = 125;
        private const int INVENTORY_HEIGHT = 600;
        private const int TIMER_DISTANCE = 30;
        private const int TIMER_BACKGROUND_OFFSET = 20;
        private PlayingState currentState = PlayingState.Playing;

        UIComponent overlay;
        PlayingLevel level;
        SortedList inventoryUI;
        Textbox timerUI;

        TimeSpan elapsedTime;

        public PlayingLevelState()
        {
            InitOverlay();
        }

        public void StartLevel(PlayingLevel lvl)
        {
            level = lvl;
            level.Player.OnPlayerAction += OnPlayerAction;
            level.Player.OnPlayerWin += ShowVictoryScreen;
            level.Player.OnPlayerLose += ShowDefeatScreen;
        }

        private void AddTimerToOverlay()
        {
            elapsedTime = TimeSpan.Zero;
            timerUI = new Textbox(CenteredLocation.All, null, "", "smallfont");
            Texture timerUIBackground = new Texture(new DirectionLocation(TIMER_DISTANCE, TIMER_DISTANCE, false), new MeasuredDimensions(timerUI, TIMER_BACKGROUND_OFFSET, TIMER_BACKGROUND_OFFSET), Utility.SolidWhiteTexture, new Color(122, 122, 122, 122));
            timerUIBackground.AddConstantComponent(timerUI);
            timerUI.PermanentInvalid = true;
            overlay.AddChild(timerUIBackground);
        }

        private void AddInventoryToOverlay()
        {
            inventoryUI = new SortedList(new CenteredLocation(0, verticalCenter: true), new SimpleDimensions(INVENTORY_WIDTH, INVENTORY_HEIGHT), backgroundColor: new Color(122, 122, 122, 122));
            overlay.AddChild(inventoryUI);
        }

        private void InitOverlay()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
            AddInventoryToOverlay();
            AddTimerToOverlay();
        }

        private void OnPlayerAction(PlayerGameObject player)
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            inventoryUI.Reset();
            foreach (InventoryItem item in level.Player.Inventory.Items)
            {
                inventoryUI.AddChild(new UISpriteSheet(new CenteredLocation(horizontalCenter: true), new SimpleDimensions(80, 80), new string[] { "KeyOverlay" }, item.type.ToKeyColorType().ToColor(), false));
            }
        }

        private void ShowGameOverState(string backgroundName)
        {
            UIComponent gameOverStateOverlay = ((GameEnvironment.GameStateManager.GetGameState("GameOverState") as GameOverState) as GameOverState).Overlay;
            gameOverStateOverlay.AddChild(new Background(new SpriteSheet(backgroundName).Sprite));
            //add the child to the overlay of the GameOverState here because you need the currrentstate.
            gameOverStateOverlay.AddChild(new SpriteSheetButton(new SimpleLocation(600, 600), null, currentState.ToString(), (UIComponent o) =>
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState")));
            GameEnvironment.GameStateManager.SwitchTo("GameOverState");
        }

        private void ShowVictoryScreen(PlayerGameObject player)
        {
            currentState = PlayingState.Victory;
            ShowGameOverState("winScreenOverlay");
        }

        private void ShowDefeatScreen(PlayerGameObject player)
        {
            currentState = PlayingState.Defeat;
            ShowGameOverState("loseScreenOverlay");
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level.HandleInput(inputHelper);
            overlay.HandleInput(inputHelper);
            level.Tiles.UpdateGraphics();
        }

        public void Update(GameTime gameTime)
        {
            level.Update(gameTime);
            overlay.Update(gameTime);

            elapsedTime = elapsedTime.Add(gameTime.ElapsedGameTime);

            timerUI.Text = elapsedTime.Minutes + ":" + elapsedTime.Seconds.ToString("00");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level.Draw(gameTime, spriteBatch);
            overlay.Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
            overlay.Reset();
            InitOverlay();
        }
    }
}
