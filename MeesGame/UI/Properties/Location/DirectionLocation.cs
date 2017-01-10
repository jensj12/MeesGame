using Microsoft.Xna.Framework;

namespace MeesGame
{
    /// <summary>
    /// Positions the UIElement it is assigned to relative to one of the four colors of its parent.
    /// </summary>
    class DirectionLocation : SimpleLocation
    {
        public bool leftToRight;
        public bool topToBottom;

        public override int X(UIComponent component)
        {
            if (leftToRight)
                return base.X(component);
            return (component.Parent?.CurrentDimensions.X ?? 0) - component.CurrentDimensions.X - base.X(component);
        }

        public override int Y(UIComponent component)
        {
            if (topToBottom)
                return base.Y(component);
            return (component.Parent?.CurrentDimensions.Y ?? 0) - component.CurrentDimensions.Y - base.Y(component);
        }

        public DirectionLocation(Point offset, bool leftToRight = true, bool topToBottom = true) : base(offset)
        {
            this.leftToRight = leftToRight;
            this.topToBottom = topToBottom;
        }

        public DirectionLocation(int xOffset = 0, int yOffset = 0, bool leftToRight = true, bool topToBottom = true) : base(xOffset, yOffset)
        {
            this.leftToRight = leftToRight;
            this.topToBottom = topToBottom;
        }
    }
}
