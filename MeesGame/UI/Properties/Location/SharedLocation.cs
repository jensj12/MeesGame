using System;
using System.Collections.Generic;

namespace MeesGame
{
    class SharedLocation : Location
    {
        List<UIComponent> sharedComponents;

        int widthOffset;
        int heightOffset;

        public override int X(UIComponent component)
        {
            int avaibleSharedWidth = (component.Parent?.CurrentDimensions.X ?? GameEnvironment.Screen.X) + widthOffset;
            int sharedComponentWidth = 0;
            foreach (UIComponent sharedComponent in sharedComponents)
                sharedComponentWidth += sharedComponent.CurrentDimensions.X;
            int sharedComponentDistance = (avaibleSharedWidth - sharedComponentWidth) / ((sharedComponents.Count > 1) ? sharedComponents.Count - 1 : 1); 
            int componentIndex = sharedComponents.IndexOf(component);

            int x = 0;

            if (componentIndex > 0)
                x += (int)(sharedComponents[componentIndex - 1].CurrentDimensions.X + sharedComponents[componentIndex - 1].CurrentRelativeLocation.X + sharedComponentDistance);
            else
                x = -widthOffset / 2;

            return x;
        }

        public override int Y(UIComponent component)
        {
            int avaibleSharedHeight = (component.Parent?.CurrentDimensions.Y ?? GameEnvironment.Screen.Y) + widthOffset;
            int sharedComponentHeight = 0;
            foreach (UIComponent sharedComponent in sharedComponents)
                sharedComponentHeight += sharedComponent.CurrentDimensions.Y;
            int sharedComponentDistance = (avaibleSharedHeight - sharedComponentHeight) / ((sharedComponents.Count > 1) ? sharedComponents.Count - 1 : 1); 
            int componentIndex = sharedComponents.IndexOf(component);

            int y = 0;

            if (componentIndex > 0)
                y += (int)(sharedComponents[componentIndex - 1].CurrentDimensions.Y + sharedComponents[componentIndex - 1].CurrentRelativeLocation.Y + sharedComponentDistance);
            else
                y = -heightOffset / 2;

            return y;
        }

        public SharedLocation(List<UIComponent> sharedComponents, int widthOffset = 0, int heightOffset = 0)
        {
            this.sharedComponents = sharedComponents;
            this.widthOffset = widthOffset;
            this.heightOffset = heightOffset;
        }
    }
}
