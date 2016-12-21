using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UIObjects;

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        private const int inventoryWidth = 125;
        private const int inventoryHeightOffset = 75;

        UIContainer overlay;
        PlayingLevel level;
        UIList inventoryUI;

        public PlayingLevelState()
        {
            initOverlay();
        }

        public void StartLevel(PlayingLevel lvl)
        {
            level = lvl;
            level.Player.PlayerAction += OnPlayerAction;
        }

        private void initOverlay()
        {
            overlay = new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());

            inventoryUI = new UIList(new Vector2(0, inventoryHeightOffset), new Vector2(inventoryWidth, (int)GameEnvironment.Screen.Y - 2 * inventoryHeightOffset), backgroundColor: new Color(122, 122, 122, 122));

            overlay.AddChild(inventoryUI);
        }

        private void OnPlayerAction(PlayerAction action)
        {
            UpdateInventoryUI();
            CheckIfVictory();
        }

        private void UpdateInventoryUI()
        {
            inventoryUI.Reset();

            foreach (InventoryItem item in level.Player.Inventory.Items)
            {
                inventoryUI.AddChild(new ImageView(Vector2.Zero, new Vector2(inventoryWidth), InventoryItem.inventoryItemAsset(item.type)));
            }
        }

        public void CheckIfVictory()
        {
            if (level.Player.CurrentTile.GetType() == typeof(EndTile))
            {
                overlay.Reset();

                overlay.AddChild(new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2(), Color.Black));

                overlay.AddChild(new Button(new Vector2(600, 400), null, Strings.victory, (UIObject o) =>
                    GameEnvironment.GameStateManager.SwitchTo("TitleMenuState")));
            }
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level.HandleInput(inputHelper);
            overlay.HandleInput(inputHelper);
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
            level.Reset();
            overlay.Reset();
        }
    }
}