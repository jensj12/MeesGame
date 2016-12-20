using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        const int tilesListWidth = 150;
        const int tilePropertiesListWidth = 200;
        const int buttonDistanceFromRightWall = 20;
        readonly Color listsBackgroundColor = new Color(122, 122, 122, 255);

        /// <summary>
        ///Ordered list of tiletypes to match the buttons with.
        /// </summary>
        List<TileType> tileTypeList;

        /// <summary>
        /// Index of the currently selected tileType as in the tileTypeList
        /// </summary>
        int selectedTileIndex;

        List<Level> level;

        int currentLevelIndex;

        /// <summary>
        /// The overlay contains all of the UI, including the tilesList and tilePropertiesList
        /// The tilesList contains all different types of tyles
        /// The tilePropertiesList contains the editable properties of the Tile on which the Editorplayer is standing
        /// </summary>
        private UIContainer overlay;
        private UIList tilesList;
        private UIList tilePropertiesList;
        private Button saveLevel;
        private Button loadLevel;

        /// <summary>
        /// State for editing levels
        /// </summary>
        public LevelEditorState()
        {
            level = new List<Level>();

            //Resize and reposition the level to prevent it from overlapping with the controls
            Level newLevel = new EditorLevel(0, GameEnvironment.Screen.X - (tilesListWidth + tilePropertiesListWidth), GameEnvironment.Screen.Y);
            newLevel.Position += new Vector2(tilesListWidth, 0);
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
            tilesList = new UIList(new Vector2(0, 0), new Vector2(tilesListWidth, GameEnvironment.Screen.Y), backgroundColor: listsBackgroundColor);
            tilePropertiesList = new UIList(new Vector2(GameEnvironment.Screen.X - tilePropertiesListWidth, 0), new Vector2(tilePropertiesListWidth, GameEnvironment.Screen.Y), backgroundColor: listsBackgroundColor);
            saveLevel = new Button(new Vector2(GameEnvironment.Screen.X - tilePropertiesListWidth + 20, 720), null, "save", SaveLevel);
            loadLevel = new Button(new Vector2(GameEnvironment.Screen.X - tilePropertiesListWidth + 20, 600), null, "Load", LoadLevel);

            overlay.AddChild(tilesList);
            overlay.AddChild(tilePropertiesList);
            overlay.AddChild(saveLevel);
            overlay.AddChild(loadLevel);

            tilesList.ChildClick += OnItemSelect;

            FillTilesList();
        }
        
        private void SaveLevel(UIObject o)
        {
            //TODO implement save level code!!!!
        }

        private void LoadLevel(UIObject o)
        {
            //TODO implement LoadLevel code!!!!
        }

        /// <summary>
        /// Fills the list of tiles
        /// </summary>
        private void FillTilesList()
        {
            //When we fill the list we want tiletypes to be empty
            tileTypeList = new List<TileType>();
            foreach (TileType tt in Enum.GetValues(typeof(TileType)))
            {
                tileTypeList.Add(tt);
                string[] tileBackgroundAndOverlays = Tile.GetAssetNamesFromTileType(tt);
                if (tileBackgroundAndOverlays != null)
                {
                    Button newButton = new Button(new Vector2(buttonDistanceFromRightWall), new Vector2(tilesListWidth - buttonDistanceFromRightWall * 2 - 10), "", OnItemSelect, null, overlayNames: tileBackgroundAndOverlays);
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
                level[0].Tiles.Add(Tile.CreateTileFromTileType(tileTypeList[selectedTileIndex]), playerLocation.X, playerLocation.Y);
                //We need to update the tile graphics, otherwise we might see wrongly displayed tiles (such as not connected wall tiles)
                level[0].Tiles.UpdateGraphicsToMatchSurroundings();
            }

            //When backspace is presset, we return to the TitleMenu
            if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Back))
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
            tilePropertiesList.Invalidate();
        }
    }

    public class EditableAttribute : Attribute
    {
        public bool isEditable;
    }
}