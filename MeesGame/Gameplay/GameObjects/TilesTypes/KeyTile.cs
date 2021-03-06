﻿using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace MeesGame
{
    class KeyTile : FloorTile
    {
        protected KeyColor keyColor;

        public KeyTile(int layer = 0, string id = "") : base(TileType.Key, layer, id)
        {
            secondarySpriteColor = keyColor.ToColor();
        }

        [Editor]
        public Color SecondarySpriteColor
        {
            get
            {
                return base.secondarySpriteColor;
            }
            set
            {
                base.secondarySpriteColor = value;
                keyColor = value.ToKeyColor();
                TileData tileData = Data;
                tileData.AdditionalInfo = (int)keyColor;
                Data = tileData;
            }
        }

        public override void UpdateGraphics()
        {
            if (IsVisited)
                secondarySprite = null;
        }

        public override void EnterTile(ITileFieldPlayer player)
        {
            player.Inventory.Items.Add(new InventoryKey(keyColor));
            base.EnterTile(player);
        }

        public override void UpdateToAdditionalInfo()
        {
            keyColor = (KeyColor)Data.AdditionalInfo;
            secondarySpriteColor = keyColor.ToColor();
        }

        public override void MarkVisited()
        {
            if (!IsVisited)
                GameEnvironment.AssetManager.PlaySound("key_pickup");
            base.MarkVisited();
        }

        public KeyColor KeyColor
        {
            get { return keyColor; }
        }
    }
}
