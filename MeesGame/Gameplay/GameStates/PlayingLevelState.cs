using MeesGame.UI;
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
        private const int inventoryWidth = 125;
        private const int inventoryHeight = 600;
        private const int timerDistance = 30;
        private const int timeBackgroundOffset = 20;
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
            level.Player.PlayerAction += OnPlayerAction;
            level.Player.PlayerWin += ShowVictoryScreen;
            level.Player.PlayerLose += ShowDefeatScreen;
        }

        private void InitOverlay()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            inventoryUI = new SortedList(new CenteredLocation(0, verticalCenter: true), new SimpleDimensions(inventoryWidth, inventoryHeight), backgroundColor: new Color(122, 122, 122, 122));

            timerUI = new Textbox(CenteredLocation.All, null, "", "smallfont");

            Texture timerUIBackground = new Texture(new DirectionLocation(timerDistance, timerDistance, false), new MeasuredDimensions(timerUI, timeBackgroundOffset, timeBackgroundOffset), Utility.SolidWhiteTexture, new Color(122, 122, 122, 122));

            timerUIBackground.AddConstantComponent(timerUI);

            elapsedTime = TimeSpan.Zero;

            timerUI.PermanentInvalid = true;

            overlay.AddChild(inventoryUI);

            overlay.AddChild(timerUIBackground);
        }

        private void OnPlayerAction(Player player)
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            inventoryUI.Reset();

            foreach (InventoryItem item in level.Player.Inventory.Items)
            {
                inventoryUI.AddChild(new UISpriteSheet(new CenteredLocation(horizontalCenter: true), new SimpleDimensions(80, 80), new string[] { "KeyOverlay" }));
            }
        }

        public void ShowVictoryScreen(Player player)
        {
            this.currentState = PlayingState.Victory;

            //add the child to the overlay of the GameOverState here because you need the currrentstate.
            (GameEnvironment.GameStateManager.GetGameState("GameOverState") as GameOverState).overlay.AddChild(new SpriteSheetButton(new SimpleLocation(600, 400), null, currentState.ToString(), (UIComponent o) =>
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState")));
            GameEnvironment.GameStateManager.SwitchTo("GameOverState");
        }

        public void ShowDefeatScreen(Player player)
        {
            this.currentState = PlayingState.Defeat;

            //add the child to the overlay of the GameOverState here because you need the currrentstate.
            (GameEnvironment.GameStateManager.GetGameState("GameOverState") as GameOverState).overlay.AddChild(new SpriteSheetButton(new SimpleLocation(600, 400), null, currentState.ToString(), (UIComponent o) =>
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState")));
            GameEnvironment.GameStateManager.SwitchTo("GameOverState");
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
