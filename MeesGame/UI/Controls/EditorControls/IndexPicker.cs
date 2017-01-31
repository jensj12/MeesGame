using MeesGame.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Tool to set the index of a tile that has an int parameter in the level editor.
    /// </summary>
    class IndexPicker : UIComponent
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 250;
        private const int INDICES_PER_ROW = 3;
        private const int INDEX_DISTANCE_FROM_TOP = 100;
        private const int EDGE_THICKNESS = 5;
        private const int NUMBER_OF_INDICES = 6;

        PropertyInfo indexProperty;
        private TextureButton[] indexButtons;
        TextureButton selectedButton;
        private List<int> selectableIndices;
        object indexPropertyContainingObject;

        public IndexPicker(Location location, PropertyInfo indexProperty, object indexPropertyContainingObject) : base(location, null)
        {
            this.indexProperty = indexProperty;
            this.indexPropertyContainingObject = indexPropertyContainingObject;

            InitIndices();

            indexButtons = new TextureButton[selectableIndices.Count];

            Dimensions = new SimpleDimensions(WIDTH, HEIGHT);
            AddConstantComponent(new Textbox(SimpleLocation.Zero, null, Strings.index_picker_text, DefaultUIValues.Default.DefaultEditorControlSpriteFont));

            InitIndexButtons();
        }

        /// <summary>
        /// Initialize the index buttons
        /// </summary>
        private void InitIndexButtons()
        {
            int selectedIndex = (int)indexProperty.GetValue(indexPropertyContainingObject);
            for (int i = 0; i < selectableIndices.Count; i++)
            {
                int index = selectableIndices[i];
                indexButtons[i] = CreateIndexButton(new SimpleLocation(WIDTH / INDICES_PER_ROW * (i % INDICES_PER_ROW), WIDTH / INDICES_PER_ROW * (i / INDICES_PER_ROW) + INDEX_DISTANCE_FROM_TOP), index, isSelected: index == selectedIndex);
                AddConstantComponent(indexButtons[i]);
            }
        }

        /// <summary>
        /// Creates a button for selecting the specified color
        /// </summary>
        /// <param name="location">The location for the button</param>
        /// <param name="index">The index that this button is for</param>
        /// <param name="isSelected">Whether the button is the selected one by default</param>
        /// <returns></returns>
        private TextureButton CreateIndexButton(Location location, int index, bool isSelected)
        {
            TextureButton indexButton = new IndexButton(location, index);
            if (isSelected)
            {
                selectedButton = indexButton;
                indexButton.Selected = true;
            }
            indexButton.Click += OnIndexSelected;
            return indexButton;
        }

        private void OnIndexSelected(UIComponent Component)
        {
            if (selectedButton != null)
                selectedButton.Selected = false;
            selectedButton = (TextureButton)Component;
            selectedButton.Selected = true;
            indexProperty.SetValue(indexPropertyContainingObject, int.Parse(((TextureButton)Component).Text));
        }

        private void InitIndices()
        {
            selectableIndices = new List<int>();

            for (int i = 1; i <= NUMBER_OF_INDICES; i++)
            {
                selectableIndices.Add(i);
            }
        }

        private class IndexButton : TextureButton
        {
            public IndexButton(Location location, int index) : base(location, new SimpleDimensions(WIDTH / INDICES_PER_ROW, WIDTH / INDICES_PER_ROW), index.ToString(), Utility.SolidWhiteTexture, Color.Black, edgeThickness: EDGE_THICKNESS)
            {
            }
        }
    }
}
