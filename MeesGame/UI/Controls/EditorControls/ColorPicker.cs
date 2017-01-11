using MeesGame.UI;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace MeesGame
{
    /// <summary>
    /// Has a static size of 200 * 300.
    /// </summary>
    class ColorPicker : UIComponent
    {
        private const int width = 200;
        private const int height = 200;
        private const int colorsPerRow = 4;
        private const int colorsDistanceFromTop = 100;

        private Color[] ConstColors = new Color[]
        {
            Color.Blue,
            Color.Red,
            Color.Green,
            Color.Cyan,
            Color.Magenta,
            Color.Yellow,
            Color.Brown,
            Color.Gold
        };

        /// <summary>
        /// The colors from which the editor can choose.
        /// </summary>
        private TextureButton[] colors = new TextureButton[8];

        /// <summary>
        /// The color(button) that is currently selected.
        /// </summary>
        TextureButton selectedButton;

        /// <summary>
        /// The Color property which we want to change with the colorPicker.
        /// </summary>
        PropertyInfo ColorProperty;

        /// <summary>
        /// The class containing the Color property.
        /// </summary>
        object colorPropertyContainingObject;

        public ColorPicker(Location location, PropertyInfo ColorProperty, object colorPropertyContainingObject) : base(location, null)
        {
            this.ColorProperty = ColorProperty;
            this.colorPropertyContainingObject = colorPropertyContainingObject;

            Dimensions = new SimpleDimensions(width, height);

            AddConstantComponent(new Textbox(SimpleLocation.Zero, null, Strings.color_picker_text, DefaultUIValues.Default.DefaultEditorControlSpriteFont));

            for(int i = 0; i < 8; i++)
            {
                Color color = ConstColors[i];
                colors[i] = new TextureButton(new SimpleLocation(width / colorsPerRow * (i % colorsPerRow), width / colorsPerRow * (i / colorsPerRow) + colorsDistanceFromTop), new SimpleDimensions(width / colorsPerRow, width / colorsPerRow), "", Utility.SolidWhiteTexture, color);
                Color tileColor = (Color)ColorProperty.GetValue(colorPropertyContainingObject);
                if (color == tileColor)
                {
                    selectedButton = colors[i];
                    colors[i].Selected = true;
                }
                colors[i].Click += OnColorClick;
                AddConstantComponent(colors[i]);
            }
        }

        /// <summary>
        /// Called when a color is clicked by that collor.
        /// </summary>
        /// <param name="Component">Button containing the color.</param>
        private void OnColorClick(UIComponent Component)
        {
            if(selectedButton != null)
                selectedButton.Selected = false;
            selectedButton = (TextureButton)Component;
            selectedButton.Selected = true;
            ColorProperty.SetValue(colorPropertyContainingObject, ((TextureButton)Component).Color);
        }
    }
}
