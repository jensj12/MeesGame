using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    class PlayingLevelState : IGameLoopObject
    {
        private const int inventoryWidth = 125;
        private const int inventoryHeightOffset = 75;

        GUIContainer overlay;
        List<PlayingLevel> level;
        int currentLevelIndex;
        GUIList inventoryUI;

        public PlayingLevelState()
        {
            level = new List<PlayingLevel>();
            level.Add(new PlayingLevel());

            initOverlay();

            currentLevelIndex = 0;
        }

        private void initOverlay()
        {
            overlay = new GUIContainer(Vector2.Zero, GameEnvironment.Screen.ToVector2());

            inventoryUI = new GUIList(new Vector2(0, inventoryHeightOffset), new Vector2(inventoryWidth, (int) GameEnvironment.Screen.Y - 2 * inventoryHeightOffset), backgroundColor: new Color(122, 122, 122, 122));

            overlay.AddChild(inventoryUI);
        }

        private void UpdateInventoryUI()
        {
            inventoryUI.Reset();

//          for (level[currentLevelIndex].Player.Inventory)
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
