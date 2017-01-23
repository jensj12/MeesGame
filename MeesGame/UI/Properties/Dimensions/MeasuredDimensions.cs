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
            if (measuredDimensionsComponent == null)
                measuredDimensionsComponent = (IMeasuredDimensions)component;
            return (int)measuredDimensionsComponent.MeasuredDimensions().X + base.Width(component);
        }

        public override int Height(UIComponent component)
        {
            if (measuredDimensionsComponent == null)
                measuredDimensionsComponent = (IMeasuredDimensions)component;
            return (int)measuredDimensionsComponent.MeasuredDimensions().Y + base.Height(component);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="measuredDimensionsComponent">If left to 0, assumes the measuredDimensionsComponent is the component calling the method</param>
        /// <param name="widthOffset"></param>
        /// <param name="heightOffset"></param>
        public MeasuredDimensions(IMeasuredDimensions measuredDimensionsComponent = null, int widthOffset = 0, int heightOffset = 0) : base(widthOffset, heightOffset)
        {
            this.measuredDimensionsComponent = measuredDimensionsComponent;
        }
    }
}
