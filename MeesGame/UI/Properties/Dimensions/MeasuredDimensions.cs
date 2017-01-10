namespace MeesGame
{
    /// <summary>
    /// Uses the dimensions as measured by the component given by the IMeasuredDimensionsComponent in the constructor.
    /// </summary>
    class MeasuredDimensions : Dimensions
    {
        IMeasuredDimensions measuredDimensionsComponent;

        public override int Width(UIComponent component)
        {
            int measured = (int)measuredDimensionsComponent.MeasuredDimensions().X;
            return measured;
        }

        public override int Height(UIComponent component)
        {
            return (int)measuredDimensionsComponent.MeasuredDimensions().Y;
        }

        public MeasuredDimensions(IMeasuredDimensions measuredDimensionsComponent)
        {
            this.measuredDimensionsComponent = measuredDimensionsComponent;
        }
    }
}
