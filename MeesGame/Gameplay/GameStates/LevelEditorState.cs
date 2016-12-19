using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        //The width of the bars with buttons the editor can click to place certain tiles
        const int leftBarWidth = 150;
        const int rightBarWidth = 200;
        const int buttonDistanceFromRightWall = 20;
        readonly Color listsBackgroundColor = new Color(122, 122, 122, 255);

        List<Level> level;

        //We need an ordered list of tiletypes to match the buttons with
        List<TileType> tileTypes;
        int currentLevelIndex;

        private UIContainer overlay;
        private UIList tilesList;
        private UIList tilesPropertiesList;

        int selectedTileIndex;

        /// <summary>
        /// State for editing levels
        /// </summary>
        public LevelEditorState()
        {
            level = new List<Level>();

            //Resize and reposition the level to prevent it from overlapping with the controls
            Level newLevel = new EditorLevel(0, GameEnvironment.Screen.X - (leftBarWidth + rightBarWidth), GameEnvironment.Screen.Y);
            newLevel.Position += new Vector2(leftBarWidth, 0);
            level.Add(newLevel);
            currentLevelIndex = 0;

            level[currentLevelIndex].Player.OnPlayerAction += PlayerMoved;

            InitUI();
        }

        /// <summary>
        /// Initializes the left and right bars so that the editor can select which tiles to edit and their properties
        /// </summary>
        private void InitUI()
        {
            overlay = new UIContainer(null, GameEnvironment.Screen.ToVector2());
            tilesList = new UIList(new Vector2(0, 0), new Vector2(leftBarWidth, GameEnvironment.Screen.Y), backgroundColor: listsBackgroundColor);
            tilesPropertiesList = new UIList(new Vector2(GameEnvironment.Screen.X - rightBarWidth, 0), new Vector2(rightBarWidth, GameEnvironment.Screen.Y), backgroundColor: listsBackgroundColor);

            overlay.AddChild(tilesList);
            overlay.AddChild(tilesPropertiesList);

            tilesList.onItemClick += OnItemSelect;

            FillTilesList();
        }

        /// <summary>
        /// Fills the list of tiles
        /// </summary>
        private void FillTilesList()
        {
            //When we fill the list we want tiletypes to be empty
            tileTypes = new List<TileType>();
            foreach (TileType tt in Enum.GetValues(typeof(TileType)))
            {
                tileTypes.Add(tt);
                string[] tileBackgroundAndOverlays = Tile.GetAssetNamesFromTileType(tt);
                if (tileBackgroundAndOverlays != null)
                {
                    Button newButton = new Button(new Vector2(buttonDistanceFromRightWall), new Vector2(leftBarWidth - buttonDistanceFromRightWall * 2 - 10), "", OnItemSelect, null, overlayNames: tileBackgroundAndOverlays);
                    tilesList.AddChild(newButton);
                }
            }
            ((Button)tilesList.Children[0]).Selected = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level[currentLevelIndex].Draw(gameTime, spriteBatch);
            overlay.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Method called when an object in the tilesList is selected. For now, only buttons can be clicked.
        /// </summary>
        /// <param name="o">The button that was clicked</param>
        public void OnItemSelect(UIObject o)
        {
            ((Button)tilesList.Children[selectedTileIndex]).Selected = false;
            selectedTileIndex = tilesList.Children.IndexOf(o);
            ((Button)tilesList.Children[selectedTileIndex]).Selected = true;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            level[currentLevelIndex].HandleInput(inputHelper);
            overlay.HandleInput(inputHelper);

            //When space is pressed, we set a tile
            if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Point playerLocation = level[currentLevelIndex].Player.Location;
                level[0].Tiles.Add(Tile.CreateTileFromTileType(tileTypes[selectedTileIndex]), playerLocation.X, playerLocation.Y);
                //We need to update the tile graphics, otherwise we might see wrongly displayed tiles (such as not connected wall tiles) 
                level[0].Tiles.UpdateGraphicsToMatchSurroundings();
            }

            if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
                GameEnvironment.GameStateManager.SwitchTo("TitleMenuState");
        }

        public void Reset()
        {
            level[currentLevelIndex].Reset();
            overlay.Reset();
        }

        public void Update(GameTime gameTime)
        {
            level[currentLevelIndex].Update(gameTime);
            overlay.Update(gameTime);
        }

        public void PlayerMoved(PlayerAction action)
        {
            Tile currentTile = level[currentLevelIndex].Player.Location
            foreach
        }
    }

    public class EditableAttribute : Attribute
    {
        public bool isEditable;
    }
}