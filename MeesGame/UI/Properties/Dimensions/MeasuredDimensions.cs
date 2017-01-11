namespace MeesGame
{
    /// <summary>
    /// Uses the dimensions as measured by the component given by the IMeasuredDimensionsComponent in the constructor.
    /// </summary>
    class MeasuredDimensions : SimpleDimensions
    {
        IMeasuredDimensions measuredDimensionsComponent;

        public override int Width(UIComponent component)
        {
            return (int)measuredDimensionsComponent.MeasuredDimensions().X + base.Width(component);
        }

        public override int Height(UIComponent component)
        {
            return (int)measuredDimensionsComponent.MeasuredDimensions().Y + base.Height(component);
        }

        public MeasuredDimensions(IMeasuredDimensions measuredDimensionsComponent, int widthOffset = 0, int heightOffset = 0): base(widthOffset, heightOffset)
        {
            this.measuredDimensionsComponent = measuredDimensionsComponent;
        }
    }
}
