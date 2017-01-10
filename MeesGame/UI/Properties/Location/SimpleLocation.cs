using Microsoft.Xna.Framework;

namespace MeesGame
{
    /// <summary>
    /// Position that stores the location relative to the parent.
    /// </summary>
    class SimpleLocation : Location
    {
        int relativeX;
        int relativeY;

        public override int X(UIComponent component)
        {
            return relativeX;
        }

        public override int Y(UIComponent component)
        {
            return relativeY;
        }

        /// <summary>
        /// Creates a RelativePosition.
        /// </summary>
        /// <param name="relativeX">X position relative to the Parent.</param>
        /// <param name="relativeY">Y position relative to the Parent.</param>
        public SimpleLocation(int relativeX = 0, int relativeY = 0)
        {
            this.relativeX = relativeX;
            this.relativeY = relativeY;
        }

        public SimpleLocation(Point relativeLocation)
        {
            this.relativeX = relativeLocation.X;
            this.relativeY = relativeLocation.Y;
        }

        public static SimpleLocation Zero
        {
            get { return new SimpleLocation(); }
        }
    }
}
