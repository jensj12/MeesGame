using MeesGame.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MeesGame
{
    /// <summary>
    /// Has a static size of 200 * 300.
    /// </summary>
    class ColorPicker : UIComponent
    {
        private const int width = 200;
        private const int height = 250;
        private const int colorsPerRow = 3;
        private const int colorsDistanceFromTop = 100;


        /// <summary>
        /// The colors from which the editor can choose.
        /// </summary>
        private TextureButton[] colorButtons;

        private List<Color> colors;

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

            InitKeyColors();

            colorButtons = new TextureButton[colors.Count];

            Dimensions = new SimpleDimensions(width, height);

            AddConstantComponent(new Textbox(SimpleLocation.Zero, null, Strings.color_picker_text, DefaultUIValues.Default.DefaultEditorControlSpriteFont));

            for (int i = 0; i < colors.Count; i++)
            {
                Color color = colors[i];
                colorButtons[i] = new TextureButton(new SimpleLocation(width / colorsPerRow * (i % colorsPerRow), width / colorsPerRow * (i / colorsPerRow) + colorsDistanceFromTop), new SimpleDimensions(width / colorsPerRow, width / colorsPerRow), "", Utility.SolidWhiteTexture, color);
                Color tileColor = (Color)ColorProperty.GetValue(colorPropertyContainingObject);
                if (color == tileColor)
                {
                    selectedButton = colorButtons[i];
                    colorButtons[i].Selected = true;
                }
                colorButtons[i].Click += OnColorClick;
                AddConstantComponent(colorButtons[i]);
            }
        }

        /// <summary>
        /// Called when a color is clicked by that collor.
        /// </summary>
        /// <param name="Component">Button containing the color.</param>
        private void OnColorClick(UIComponent Component)
        {
            if (selectedButton != null)
                selectedButton.Selected = false;
            selectedButton = (TextureButton)Component;
            selectedButton.Selected = true;
            ColorProperty.SetValue(colorPropertyContainingObject, ((TextureButton)Component).Color);
        }

        /// <summary>
        /// Creates a list of the colors that can be used.
        /// </summary>
        private void InitKeyColors()
        {
            colors = new List<Color>();

            int amountOfColors = Enum.GetValues(typeof(KeyColor)).Length;

            foreach (KeyColor color in Enum.GetValues(typeof(KeyColor)))
            {
                colors.Add(color.ToColor());
            }

        }
    }
}
