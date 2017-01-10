namespace MeesGame
{
    /// <summary>
    /// Inherits the dimensions from the parent of the UIComponent it is assigned to.
    /// </summary>
    class InheritDimensions : SimpleDimensions
    {
        private bool inheritWidth;
        private bool inheritHeight;

        public override int Height(UIComponent component)
        {
            if (inheritHeight)
                return component.Parent?.CurrentDimensions.Y ?? GameEnvironment.Screen.Y - base.Height(component);
            return base.Height(component);
        }

        public override int Width(UIComponent component)
        {
            if (inheritWidth)
                return component.Parent?.CurrentDimensions.X ?? GameEnvironment.Screen.X - base.Width(component);
            return base.Width(component);
        }

        public InheritDimensions(bool inheritWidth = false, bool inheritHeight = false, int x = 0, int y = 0) : base(x, y)
        {
            this.inheritWidth = inheritWidth;
            this.inheritHeight = inheritHeight;
        }

        public static InheritDimensions All
        {
            get { return new InheritDimensions(true, true); }
        }
    }
}
