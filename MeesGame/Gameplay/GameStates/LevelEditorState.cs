using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace MeesGame
{
    internal class LevelEditorState : IGameLoopObject
    {
        const int TILES_LIST_WIDTH = 150;
        const int PROPERTIES_LIST_WIDTH = 200;
        const int BUTTON_DISTANCE_FROM_TOP = 20;

        /// <summary>
        /// Ordered list of tiletypes to match the buttons with.
        /// </summary>
        List<TileType> tileTypeList;

        /// <summary>
        /// Index of the currently selected tileType as in the tileTypeList
        /// </summary>
        int selectedTileIndex;

        EditorLevel level;

        /// <summary>
        /// Contains all of the UI, including the tilesList and tilePropertiesList
        /// </summary>
        private UIComponent overlay;

        /// <summary>
        /// Contains all different types of tiles
        /// </summary>
        private SortedList tilesList;

        /// <summary>
        /// Contains the editable properties of the Tile on which the Editorplayer is standing
        /// </summary>
        private SortedList tilePropertiesList;

        private Button newLevel;
        private Button saveLevel;
        private Button loadLevel;

        private NewLevelBox newLevelBox;

        /// <summary>
        /// State for editing levels
        /// </summary>
        public LevelEditorState()
        {
            GameEnvironment.ScreenChanged += OnScreenSizeChanged;

            InitLevel();

            InitUI();
        }

        private TileField CreateTileField()
        {
            TileField tf;
            if (newLevelBox == null)
                tf = new TileField(Level.DEFAULT_NUM_ROWS, Level.DEFAULT_NUM_COLS);
            else
                tf = new TileField(newLevelBox.Rows, newLevelBox.Columns);
            EditorLevel.FillWithEmptyTiles(tf);
            return tf;
        }

        private void InitLevel(TileField tf = null)
        {
            if (tf == null) tf = CreateTileField();

            //Resize and reposition the level to prevent it from overlapping with the controls
            level = new EditorLevel(tf, 0, VisibleLevelArea);
            level.Position += new Vector2(TILES_LIST_WIDTH, 0);
            level.Player.OnMove += PlayerMoved;
        }

        private void InitNewLevelBox()
        {
            newLevelBox = new NewLevelBox(CenteredLocation.All, level.Tiles.Rows, level.Tiles.Columns);
            newLevelBox.Succes += (UIComponent component) =>
            {
                InitLevel();
                newLevelBox.Visible = false;
            };
            newLevelBox.Visible = false;
        }

        /// <summary>
        /// Initializes the left and right bars so that the editor can select which tiles to edit and their properties
        /// </summary>
        private void InitUI()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
            tilesList = new SortedList(SimpleLocation.Zero, new InheritDimensions(false, true, TILES_LIST_WIDTH));
            tilePropertiesList = new SortedList(new DirectionLocation(leftToRight: false), new InheritDimensions(false, true, PROPERTIES_LIST_WIDTH));
            newLevel = new SpriteSheetButton(new DirectionLocation(10, 230, false, false), null, Strings.newmaze, (UIComponent component) =>
            {
                newLevelBox.Visible = true;
            });
            loadLevel = new SpriteSheetButton(new CombinationLocation(new DirectionLocation(10, 0, false), new RelativeToLocation(newLevel, yOffset: 10, relativeToTop: false)), null, Strings.load, LoadLevel);
            saveLevel = new SpriteSheetButton(new CombinationLocation(new DirectionLocation(10, 0, false), new RelativeToLocation(loadLevel, yOffset: 10, relativeToTop: false)), null, Strings.save, SaveLevel);

            InitNewLevelBox();

            overlay.AddChild(tilesList);
            overlay.AddChild(tilePropertiesList);
            overlay.AddChild(newLevelBox);
            overlay.AddChild(newLevel);
            overlay.AddChild(saveLevel);
            overlay.AddChild(loadLevel);

            tilesList.ChildClick += OnItemSelect;

            FillTilesList();
        }

        private void SaveLevel(UIComponent o)
        {
            FileIO.Save(level.Tiles);
        }

        private void LoadLevel(UIComponent o)
        {
            try
            {
                InitLevel(FileIO.ShowLoadFileDialog());
                PlayerMoved(level.Player);
            }
            catch (Exception) { }
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
                    Button newButton = new SpriteSheetButton(new CenteredLocation(yOffset: BUTTON_DISTANCE_FROM_TOP, horizontalCenter: true), new SimpleDimensions(TILES_LIST_WIDTH - BUTTON_DISTANCE_FROM_TOP * 2 - 10, TILES_LIST_WIDTH - BUTTON_DISTANCE_FROM_TOP * 2 - 10), "", OnItemSelect, null, tileBackgroundAndOverlays, tiled: false);
                    tilesList.AddChild(newButton);
                }
            }
            ((Button)tilesList.Children[0]).Selected = true;
            PlayerMoved(level.Player);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            level.Draw(gameTime, spriteBatch);
            overlay.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Method called when an object in the tilesList is selected. For now, only buttons can be clicked.
        /// </summary>
        /// <param name="o">The button that was clicked</param>
        private void OnItemSelect(UIComponent o)
        {
            ((Button)tilesList.Children[selectedTileIndex]).Selected = false;
            selectedTileIndex = tilesList.Children.IndexOf(o);
            ((Button)tilesList.Children[selectedTileIndex]).Selected = true;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            //prevents the player from moving when the newlevelbox is visible.
            if (!newLevelBox.Visible)
                level.HandleInput(inputHelper);
            else if (inputHelper.MouseLeftButtonPressed() && !newLevelBox.AbsoluteRectangle.Contains(inputHelper.MousePosition))
                newLevelBox.Visible = false;


            overlay.HandleInput(inputHelper);

            //When space is pressed, we set a tile
            if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                if (tileTypeList[selectedTileIndex] == TileType.Start)
                {
                    if (level.Tiles.UpdateStart())
                    {
                        return;
                    }
                }
                Point playerLocation = level.Player.Location;
                Tile CurrentTile = Tile.CreateTileFromTileType(tileTypeList[selectedTileIndex]);
                level.Tiles.Add(CurrentTile, playerLocation.X, playerLocation.Y);
                //We need to update the tile graphics, otherwise we might see wrongly displayed tiles (such as not connected wall tiles)
                level.Tiles.UpdateGraphicsToMatchSurroundings(playerLocation);
                CurrentTileChanged(level.Tiles.GetTile(playerLocation));
            }
        }

        public void Reset()
        {
        }

        public void Update(GameTime gameTime)
        {
            level.Update(gameTime);
            overlay.Update(gameTime);
        }

        public void OnScreenSizeChanged(object o, EventArgs args)
        {
            level.Camera.SetScreenSize(VisibleLevelArea);
        }

        /// <summary>
        /// Returns the dimensions which fill the visible level.
        /// </summary>
        private Point VisibleLevelArea
        {
            get { return new Point(GameEnvironment.Screen.X - (TILES_LIST_WIDTH + PROPERTIES_LIST_WIDTH), GameEnvironment.Screen.Y); }
        }

        /// <summary>
        /// Called every time the players moves to update the UI accordingly.
        /// </summary>
        /// <param name="player">The player that moved.</param>
        private void PlayerMoved(EditorPlayer player)
        {
            if (level.Tiles.OutOfTileField(player.Location)) return;
            Tile playerTile = level.Tiles.GetTile(player.Location);
            CurrentTileChanged(playerTile);
        }

        /// <summary>
        /// Called when the Tile that the editor uses should change
        /// </summary>
        /// <param name="playerTile">Tile the player is standing on.</param>
        private void CurrentTileChanged(Tile playerTile)
        {
            tilePropertiesList.Reset();
            PropertyInfo[] properties = playerTile.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                foreach (EditorAttribute editorAttribute in property.GetCustomAttributes(true))
                {
                    UIComponent editorComponent = GetEditorControl(playerTile, property);
                    if (editorComponent != null)
                        tilePropertiesList.AddChild(editorComponent);
                }
            }
        }

        /// <summary>
        /// Returns the UI component that can edit the property
        /// </summary>
        /// <param name="tile">The tile containing the property.</param>
        /// <param name="property">The property to be changed.</param>
        /// <returns></returns>
        private static UIComponent GetEditorControl(Tile tile, PropertyInfo property)
        {
            if (property.GetMethod.ReturnType == typeof(Color))
            {
                return new ColorPicker(SimpleLocation.Zero, property, tile);
            }
            else if (property.GetMethod.ReturnType == typeof(int))
            {
                return new IndexPicker(SimpleLocation.Zero, property, tile);
            }
            return null;
        }
    }
}
