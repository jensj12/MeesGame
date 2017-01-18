using Microsoft.Xna.Framework;

namespace MeesGame
{
    class WrapperDimensions : SimpleDimensions
    {
        bool wrapX, wrapY;

        /// <summary>
        /// When the parent relies on the child's dimensions and vice versa, the parent will have to return 0 as its dimensions to prevent
        /// an infinite loop.
        /// </summary>
        bool calculatingDimensions = false;

        public override int Width(UIComponent component)
        {
            if (calculatingDimensions)
                return 0;
            calculatingDimensions = true;

            //Stores the deference between the element furthest to the left and the one furthest to the right.
            int smallestLeft = 0, hbiggestRight = 0;

            if (wrapX)
            {
                foreach (UIComponent child in component.Children)
                {
                    Rectangle childRectangle = child.RelativeRectangle;
                    if (childRectangle.Left < smallestLeft)
                        smallestLeft = childRectangle.Left;
                    if (childRectangle.Right > hbiggestRight)
                        hbiggestRight = childRectangle.Right;
                }

                foreach (UIComponent constantComponent in component.ConstantComponents)
                {
                    Rectangle componentRectangle = constantComponent.RelativeRectangle;
                    if (componentRectangle.Left < smallestLeft)
                        smallestLeft = componentRectangle.Left;
                    if (componentRectangle.Right > hbiggestRight)
                        hbiggestRight = componentRectangle.Right;
                }
            }

            calculatingDimensions = false;
            return base.Width(component) + hbiggestRight - smallestLeft; ;
        }

        public override int Height(UIComponent component)
        {
            if (calculatingDimensions)
                return 0;
            calculatingDimensions = true;

            //Stores the deference between the element furthest to the top and the one furthest to the bottom.
            int smallesttTop = 0, biggestBottom = 0;

            if (wrapY)
            {
                foreach (UIComponent child in component.Children)
                {
                    Rectangle childRectangle = child.RelativeRectangle;
                    if (childRectangle.Top < smallesttTop)
                        smallesttTop = childRectangle.Top;
                    if (childRectangle.Bottom > biggestBottom)
                        biggestBottom = childRectangle.Bottom;
                }

                foreach (UIComponent constantComponent in component.ConstantComponents)
                {
                    Rectangle componentRectangle = constantComponent.RelativeRectangle;
                    if (componentRectangle.Top < smallesttTop)
                        smallesttTop = componentRectangle.Top;
                    if (componentRectangle.Bottom > biggestBottom)
                        biggestBottom = componentRectangle.Bottom;
                }
            }

            calculatingDimensions = false;
            return base.Width(component) + biggestBottom - smallesttTop;
        }

        public WrapperDimensions(int xOffset = 0, int yOffset = 0, bool wrapX = false, bool wrapY = false) : base(xOffset, yOffset)
        {
            this.wrapX = wrapX;
            this.wrapY = wrapY;
        }

        /// <summary>
        /// Wraps around the children without any offset.
        /// </summary>
        public static WrapperDimensions All
        {
            get { return new WrapperDimensions(0, 0, true, true); }
        }
    }
}
