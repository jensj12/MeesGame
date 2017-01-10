using Microsoft.Xna.Framework;

namespace MeesGame
{
    public abstract class Location
    {
        /// <summary>
        /// returns the X of the UIComponent given as parameter relative to the Parent UIComponent
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public abstract int X(UIComponent component);

        /// <summary>
        /// returns the Y of the UIComponent given as parameter relative to the Parent UIComponent
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public abstract int Y(UIComponent component);

        /// <summary>
        /// returns the X and Y of the UIComponent given as parameter as a Point.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public Point ToPoint(UIComponent component)
        {
            return new Point(X(component), Y(component));
        }

        /// <summary>
        /// returns the X and Y of the UIComponent given as parameter as a Vector2.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public Vector2 ToVector2(UIComponent component)
        {
            return new Vector2(X(component), Y(component));
        }

        public static implicit operator Location(Point p)
        {
            return new SimpleLocation(p.X, p.Y);
        }

        public static implicit operator Location(Vector2 v)
        {
            return new SimpleLocation((int)v.X, (int)v.Y);
        }
    }
}
