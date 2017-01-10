namespace MeesGame
{
    /// <summary>
    /// Position that stores the location relative to the parent.
    /// </summary>
    class SimpleDimensions : Dimensions
    {
        int width;
        int height;

        public override int Width(UIComponent component)
        {
            return width;
        }

        public override int Height(UIComponent component)
        {
            return height;
        }

        /// <summary>
        /// Creates a RelativePosition.
        /// </summary>
        /// <param name="relativeX">X position relative to the Parent.</param>
        /// <param name="relativeY">Y position relative to the Parent.</param>
        public SimpleDimensions(int width = 1, int height = 1)
        {
            this.width = width;
            this.height = height;
        }
    }
}
