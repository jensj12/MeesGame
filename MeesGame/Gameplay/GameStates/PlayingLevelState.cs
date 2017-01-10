using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        private const int inventoryWidth = 125;
        private const int inventoryHeight = 600;

        UIComponent overlay;
        PlayingLevel level;
        SortedList inventoryUI;

        public PlayingLevelState()
        {
            InitOverlay();
        }

        public void StartLevel(PlayingLevel lvl)
        {
            level = lvl;
            level.Player.PlayerAction += OnPlayerAction;
            level.Player.PlayerWin += ShowVictoryScreen;
        }

        private void InitOverlay()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);

            inventoryUI = new SortedList(new CenteredLocation(0, verticalCenter: true), new SimpleDimensions(inventoryWidth, inventoryHeight), backgroundColor: new Color(122, 122, 122, 122));

            overlay.AddChild(inventoryUI);
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
