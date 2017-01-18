using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    public enum KeyColor
    {
        KeyBlue,
        KeyCyan,
        KeyGreen,
        KeyMagenta,
        KeyRed,
        KeyYellow
    }

    static class KeyColorExtensions
    {
        public static KeyColor ToKeyColorType(this InventoryItemType color)
        {
            switch (color)
            {
                case InventoryItemType.KeyBlue:
                    return KeyColor.KeyBlue;
                case InventoryItemType.KeyCyan:
                    return KeyColor.KeyCyan;
                case InventoryItemType.KeyGreen:
                    return KeyColor.KeyGreen;
                case InventoryItemType.KeyMagenta:
                    return KeyColor.KeyMagenta;
                case InventoryItemType.KeyRed:
                    return KeyColor.KeyRed;
                case InventoryItemType.KeyYellow:
                    return KeyColor.KeyYellow;
                default:
                    throw new NotImplementedException();
            }
        }

        public static InventoryItemType ToInventeryItemType(this KeyColor color)
        {
            switch (color)
            {
                case KeyColor.KeyBlue:
                    return InventoryItemType.KeyBlue;
                case KeyColor.KeyCyan:
                    return InventoryItemType.KeyCyan;
                case KeyColor.KeyGreen:
                    return InventoryItemType.KeyGreen;
                case KeyColor.KeyMagenta:
                    return InventoryItemType.KeyMagenta;
                case KeyColor.KeyRed:
                    return InventoryItemType.KeyRed;
                case KeyColor.KeyYellow:
                    return InventoryItemType.KeyYellow;
                default:
                    throw new NotImplementedException();
            }
        }

        public static KeyColor FromColor(Color color)
        {
            if (color == Color.Blue) return KeyColor.KeyBlue;
            if (color == Color.Cyan) return KeyColor.KeyCyan;
            if (color == Color.Green) return KeyColor.KeyGreen;
            if (color == Color.Magenta) return KeyColor.KeyMagenta;
            if (color == Color.Red) return KeyColor.KeyRed;
            if (color == Color.Yellow) return KeyColor.KeyYellow;
            throw new NotImplementedException();
        }

        public static Color ToColor(this KeyColor color)
        {
            switch (color)
            {
                case KeyColor.KeyBlue:
                    return Color.Blue;
                case KeyColor.KeyCyan:
                    return Color.Cyan;
                case KeyColor.KeyGreen:
                    return Color.Green;
                case KeyColor.KeyMagenta:
                    return Color.Magenta;
                case KeyColor.KeyRed:
                    return Color.Red;
                case KeyColor.KeyYellow:
                    return Color.Yellow;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
