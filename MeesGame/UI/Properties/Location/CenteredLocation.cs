namespace MeesGame
{
    /// <summary>
    /// Centers the UIComponent that uses it relative to its parent.
    /// </summary>
    class CenteredLocation : SimpleLocation
    {
        bool horizontalCenter;
        bool verticalCenter;

        public override int X(UIComponent component)
        {
            if (horizontalCenter)
                return (component.Parent?.CurrentDimensions.X ?? GameEnvironment.Screen.X) / 2 - (component.CurrentDimensions.X) / 2 + base.X(component);
            return base.X(component);
        }

        public override int Y(UIComponent component)
        {
            if (verticalCenter)
                return (component.Parent?.CurrentDimensions.Y ?? GameEnvironment.Screen.Y) / 2 - (component.CurrentDimensions.Y) / 2 + base.Y(component);
            return base.Y(component);
        }

        public CenteredLocation(int xOffset = 0, int yOffset = 0, bool horizontalCenter = false, bool verticalCenter = false) : base(xOffset, yOffset)
        {
            this.horizontalCenter = horizontalCenter;
            this.verticalCenter = verticalCenter;
        }

        public static CenteredLocation All
        {
            get { return new CenteredLocation(horizontalCenter: true, verticalCenter: true); }
        }
    }
}
