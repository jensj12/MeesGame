using MeesGame.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace MeesGame
{
    /// <summary>
    /// Tool to set the index of a tile that has an int parameter in the level editor.
    /// </summary>
    class IndexPicker : UIComponent
    {
        private const int width = 200;
        private const int height = 250;
        private const int indecesPerRow = 3;
        private const int indexDistanceFromTop = 100;
        private const int edgeThickness = 5;

        PropertyInfo IndexProperty;
        private TextureButton[] indexButtons;
        TextureButton selectedButton;
        private List<int> selectableIndeces;
        object indexPropertyContainingObject;

        public IndexPicker(Location location, PropertyInfo IndexProperty, object indexPropertyContainingObject) : base(location, null)
        {
            this.IndexProperty = IndexProperty;
            this.indexPropertyContainingObject = indexPropertyContainingObject;

            InitIndices();

            indexButtons = new TextureButton[selectableIndeces.Count];

            Dimensions = new SimpleDimensions(width, height);
            AddConstantComponent(new Textbox(SimpleLocation.Zero, null, Strings.index_picker_text, DefaultUIValues.Default.DefaultEditorControlSpriteFont));

            for (int i = 0; i < selectableIndeces.Count; i++)
            {
                int index = selectableIndeces[i];
                indexButtons[i] = new TextureButton(new SimpleLocation(width / indecesPerRow * (i % indecesPerRow), width / indecesPerRow * (i / indecesPerRow) + indexDistanceFromTop),
                    new SimpleDimensions(width / indecesPerRow, width / indecesPerRow), (i + 1).ToString(), Utility.SolidWhiteTexture, Color.Black, edgeThickness: edgeThickness);
                int tileIndex = (int)IndexProperty.GetValue(indexPropertyContainingObject);
                if (index == tileIndex)
                {
                    selectedButton = indexButtons[i];
                    indexButtons[i].Selected = true;
                }
                indexButtons[i].Click += OnIndexSelected;
                AddConstantComponent(indexButtons[i]);
            }
        }

        public void OnIndexSelected(UIComponent Component)
        {
            if (selectedButton != null)
                selectedButton.Selected = false;
            selectedButton = (TextureButton)Component;
            selectedButton.Selected = true;
            IndexProperty.SetValue(indexPropertyContainingObject, int.Parse(((TextureButton)Component).Text));
        }

        private void InitIndices()
        {
            selectableIndeces = new List<int>();

            for (int i = 1; i < 7; i++)
            {
                selectableIndeces.Add(i);
            }
        }
    }
}
