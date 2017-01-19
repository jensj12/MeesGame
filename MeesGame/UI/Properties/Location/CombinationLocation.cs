using System;

namespace MeesGame
{
    class CombinationLocation : Location
    {
        public Location locationX;
        public Location locationY;

        public override int X(UIComponent component)
        {
            return locationX.X(component);
        }

        public override int Y(UIComponent component)
        {
            return locationY.Y(component);
        }

        public CombinationLocation(Location locationX, Location locationY)
        {
            this.locationX = locationX;
            this.locationY = locationY;
        }
    }
}
