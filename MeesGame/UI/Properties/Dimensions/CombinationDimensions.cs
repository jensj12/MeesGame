namespace MeesGame
{
    class CombinationDimensions : Dimensions
    {
        Dimensions widthDimensions;
        Dimensions heightDimensions;

        public override int Width(UIComponent component)
        {
            return widthDimensions.Width(component);
        }

        public override int Height(UIComponent component)
        {
            return heightDimensions.Height(component);
        }

        public CombinationDimensions(Dimensions widthDimensions, Dimensions heightDimensions)
        {
            this.widthDimensions = widthDimensions;
            this.heightDimensions = heightDimensions;
        }
    }
}
