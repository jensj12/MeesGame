namespace MeesGame
{
    class RelativeToLocation : SimpleLocation
    {
        UIComponent relativeToComponent;
        bool relativeToTop, relativeToLeft;

        public override int X(UIComponent component)
        {
            return base.X(component) + (relativeToLeft ? relativeToComponent.RelativeRectangle.Left : relativeToComponent.RelativeRectangle.Right);
        }

        public override int Y(UIComponent component)
        {
            return base.Y(component) + (relativeToTop ? relativeToComponent.RelativeRectangle.Top : relativeToComponent.RelativeRectangle.Bottom);
        }

        public RelativeToLocation(UIComponent relativeToComponent, int xOffset = 0, int yOffset = 0, bool relativeToLeft = true, bool relativeToTop = true) : base(xOffset, yOffset)
        {
            this.relativeToComponent = relativeToComponent;
            this.relativeToTop = relativeToTop;
            this.relativeToLeft = relativeToLeft;
        }
    }
}
