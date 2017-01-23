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
        const int tilesListWidth = 150;
        const int tilePropertiesListWidth = 200;
        const int buttonDistanceFromTop = 20;

        /// <summary>
        ///Ordered list of tiletypes to match the buttons with.
        /// </summary>
        List<TileType> tileTypeList;

        /// <summary>
        /// Index of the currently selected tileType as in the tileTypeList
        /// </summary>
        int selectedTileIndex;

        EditorLevel level;

        /// <summary>
        /// The overlay contains all of the UI, including the tilesList and tilePropertiesList
        /// The tilesList contains all different types of tiles
        /// The tilePropertiesList contains the editable properties of the Tile on which the Editorplayer is standing
        /// </summary>
        private UIComponent overlay;
        private SortedList tilesList;
        private SortedList tilePropertiesList;

        private Button showResizeLevelButton;
        private Button saveLevel;
        private Button loadLevel;

        private NewLevelBox newLevelBox;

        /// <summary>
        /// State for editing levels
        /// </summary>
        public LevelEditorState()
        {
            InitLevel();

            InitUI();
        }

        private void InitLevel(TileField tf = null)
        {
            if (tf == null)
            {
                if (newLevelBox == null)
                    tf = new TileField(Level.DEFAULT_NUM_ROWS, Level.DEFAULT_NUM_COLS);
                else
                    tf = new TileField(newLevelBox.Rows, newLevelBox.Columns);
                EditorLevel.FillWithEmptyTiles(tf);
            }
            //Resize and reposition the level to prevent it from overlapping with the controls
            level = new EditorLevel(tf, 0, GameEnvironment.Screen.X - (tilesListWidth + tilePropertiesListWidth), GameEnvironment.Screen.Y);
            level.Position += new Vector2(tilesListWidth, 0);


            level.Player.OnMove += PlayerMoved;

        }

        /// <summary>
        /// Initializes the left and right bars so that the editor can select which tiles to edit and their properties
        /// </summary>
        private void InitUI()
        {
            overlay = new UIComponent(SimpleLocation.Zero, InheritDimensions.All);
            tilesList = new SortedList(SimpleLocation.Zero, new SimpleDimensions(tilesListWidth, GameEnvironment.Screen.Y));
            tilePropertiesList = new SortedList(new DirectionLocation(leftToRight: false), new InheritDimensions(false, true, tilePropertiesListWidth));
            saveLevel = new SpriteSheetButton(new DirectionLocation(20, 720, false), null, Strings.save, SaveLevel);
            loadLevel = new SpriteSheetButton(new DirectionLocation(20, 600, false), null, Strings.load, LoadLevel);
            showResizeLevelButton = new SpriteSheetButton(new DirectionLocation(20, 480, false), null, Strings.newmaze, (UIComponent component) => {
                newLevelBox.Visible = true;
            });

            newLevelBox = new NewLevelBox(CenteredLocation.All, level.Tiles.Rows, level.Tiles.Columns);
            newLevelBox.Succes += (UIComponent component) => {
                InitLevel();
                newLevelBox.Visible = false;
            };

            newLevelBox.Visible = false;


            overlay.AddChild(tilesList);
            overlay.AddChild(tilePropertiesList);
            overlay.AddChild(newLevelBox);
            overlay.AddChild(showResizeLevelButton);
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
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                string directory = GameEnvironment.AssetManager.Content.RootDirectory + "/levels";
                DirectoryInfo info = Directory.CreateDirectory(directory);
                openFileDialog.InitialDirectory = info.FullName;
                openFileDialog.Filter = Strings.file_dialog_filter_lvl;
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    InitLevel(FileIO.Load(openFileDialog.FileName));
                    PlayerMoved(level.Player);
                }
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
                    Button newButton = new SpriteSheetButton(new CenteredLocation(yOffset: buttonDistanceFromTop, horizontalCenter: true), new SimpleDimensions(tilesListWidth - buttonDistanceFromTop * 2 - 10, tilesListWidth - buttonDistanceFromTop * 2 - 10), "", OnItemSelect, null, tileBackgroundAndOverlays);
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
        public void OnItemSelect(UIComponent o)
        {
            ((Button)tilesList.Children[selectedTileIndex]).Selected = false;
            selectedTileIndex = tilesList.Children.IndexOf(o);
            ((Button)tilesList.Children[selectedTileIndex]).Selected = true;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            //prevents the player from moving when the newlevelbox is visible.
            if (newLevelBox.Visible)
            {
                newLevelBox.HandleInput(inputHelper);
            }
            else
            {
                level.HandleInput(inputHelper);
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
                    CurrentTileChanged(CurrentTile);
                }
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

        /// <summary>
        /// Called every time the players moves to update the UI accordingly.
        /// </summary>
        /// <param name="player">The player that moved.</param>
        public void PlayerMoved(EditorPlayer player)
        {
            if (level.Tiles.OutOfTileField(player.Location)) return;
            Tile playerTile = (Tile)level.Tiles.Objects[player.Location.X, player.Location.Y];
            CurrentTileChanged(playerTile);
        }

        /// <summary>
        /// Called when the Tile that the editor uses should change
        /// </summary>
        /// <param name="playerTile">Tile the player is standing on.</param>
        public void CurrentTileChanged(Tile playerTile)
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
