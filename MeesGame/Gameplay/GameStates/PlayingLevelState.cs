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
        List<PlayingLevel> level;
        int currentLevelIndex;
        UIList inventoryUI;

        public PlayingLevelState()
        {
            level = new List<PlayingLevel>();
            level.Add(new PlayingLevel());

            initOverlay();

            currentLevelIndex = 0;

            level[currentLevelIndex].Player.OnPlayerAction += UpdateInventoryUI;
        }

        private void initOverlay()
        {
            overlay = new UIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());

            inventoryUI = new UIList(new Vector2(0, inventoryHeightOffset), new Vector2(inventoryWidth, (int)GameEnvironment.Screen.Y - 2 * inventoryHeightOffset), backgroundColor: new Color(122, 122, 122, 122));

            overlay.AddChild(inventoryUI);
        }

        private void UpdateInventoryUI(PlayerAction action)
        {
            inventoryUI.Reset();

            foreach (InventoryItem item in level[currentLevelIndex].Player.State.Inventory.Items)
            {
                inventoryUI.AddChild(new ImageView(Vector2.Zero, new Vector2(inventoryWidth), InventoryItem.inventoryItemAsset(item.type)));
            }
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level[currentLevelIndex].HandleInput(inputHelper);
            overlay.HandleInput(inputHelper);
        }

        public void Update(GameTime gameTime)
        {
            level[currentLevelIndex].Update(gameTime);
            overlay.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level[currentLevelIndex].Draw(gameTime, spriteBatch);
            overlay.Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
            level[currentLevelIndex].Reset();
            overlay.Reset();
        }
    }
}