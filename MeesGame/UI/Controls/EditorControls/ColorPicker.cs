using MeesGame.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Has a static size of 200 * 300.
    /// </summary>
    class ColorPicker : UIComponent
    {
        private const int WIDTH = 200;
        private const int HEIGHT = 250;
        private const int COLORS_PER_ROW = 3;
        private const int COLORS_DISTANCE_FROM_TOP = 100;
        private const int EDGE_THICKNESS = 5;


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

            Dimensions = new SimpleDimensions(WIDTH, HEIGHT);
            Textbox colorPickerText = new Textbox(SimpleLocation.Zero, null, Strings.color_picker_text, DefaultUIValues.Default.DefaultEditorControlSpriteFont);
            AddConstantComponent(colorPickerText);

            InitColorButtons();
        }

        private void InitColorButtons()
        {
            colorButtons = new TextureButton[colors.Count];
            Color selectedColor = (Color)ColorProperty.GetValue(colorPropertyContainingObject);
            for (int i = 0; i < colors.Count; i++)
            {
                colorButtons[i] = CreateColorButton(new SimpleLocation(WIDTH / COLORS_PER_ROW * (i % COLORS_PER_ROW), WIDTH / COLORS_PER_ROW * (i / COLORS_PER_ROW) + COLORS_DISTANCE_FROM_TOP), colors[i], isSelected: colors[i] == selectedColor);
                AddConstantComponent(colorButtons[i]);
            }
        }
        
        /// <summary>
        /// Creates a button for selecting the specified color
        /// </summary>
        /// <param name="location">The location for the button</param>
        /// <param name="color">The color that this button is for</param>
        /// <param name="isSelected">Whether the button is the selected one by default</param>
        /// <returns></returns>
        private TextureButton CreateColorButton(Location location, Color color, bool isSelected)
        {
            TextureButton colorButton = new ColorButton(location, color);
            if(isSelected)
            {
                selectedButton = colorButton;
                colorButton.Selected = true;
            }
            colorButton.Click += OnColorClick;
            return colorButton;
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
            foreach (KeyColor color in Enum.GetValues(typeof(KeyColor)))
            {
                colors.Add(color.ToColor());
            }
        }
        private class ColorButton : TextureButton
        {
            public ColorButton(Location location, Color color) : base(location, new SimpleDimensions(WIDTH / COLORS_PER_ROW, WIDTH / COLORS_PER_ROW), "", Utility.SolidWhiteTexture, color, edgeThickness: EDGE_THICKNESS)
            {
            }
        }
    }
}
