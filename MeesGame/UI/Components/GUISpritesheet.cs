﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    /// <summary>
    /// Uses a SpriteSheets to render itself.
    /// </summary>
    class UISpriteSheet : UIComponent
    {
        private List<SpriteSheet> spritesheets;

        Color color;

        bool tiled;

        public UISpriteSheet(Location location, Dimensions dimensions, string[] spritesheetsToAdd = null, Color? color = null, bool tiled = true) : base(location, dimensions)
        {
            spritesheets = new List<SpriteSheet>();
            AddSpritesheets(spritesheetsToAdd ?? new string[] { });

            this.color = color ?? Color.White;
            this.tiled = tiled;
        }

        public void AddSpritesheets(string[] spritesheetsToAdd)
        {
            for (int i = 0; i < spritesheetsToAdd.Length; i++)
                spritesheets.Add(new SpriteSheet(spritesheetsToAdd[i]));
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            DrawSpritesheets(spriteBatch, anchorPoint.ToPoint());
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
        }

        private void DrawSpritesheets(SpriteBatch spriteBatch, Point anchorPoint)
        {
            for (int i = 0; i < spritesheets.Count; i++)
            {
                spritesheets[i].Draw(spriteBatch, anchorPoint.ToVector2(), anchorPoint.ToVector2(), CurrentDimensions, color, tiled);
            }
        }

        protected override void InputPropertyChanged()
        {
            Invalidate();
        }

        public List<SpriteSheet> DefaultSpritesheets
        {
            get { return spritesheets; }
        }

        public bool Tiled
        {
            get { return tiled; }
            set { tiled = value; }
        }
    }
}
