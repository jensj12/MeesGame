using Microsoft.Xna.Framework;

namespace MeesGame
{
    public abstract class Dimensions
    {
        /// <summary>
        /// returns the Width of the UIComponent given as parameter.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public abstract int Width(UIComponent component);

        /// <summary>
        /// returns the Height of the UIComponent given as parameter.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public abstract int Height(UIComponent component);

        /// <summary>
        /// returns the Height and Width of the UIComponent given as parameter as a Point.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public Point ToPoint(UIComponent component)
        {
            return new Point(Width(component), Height(component));
        }

        /// <summary>
        /// returns the Height and Width of the UIComponent given as parameter as a Vector2.
        /// </summary>
        /// <param name="component">The UIComponent that call the function</param>
        /// <returns></returns>
        public Vector2 ToVector2(UIComponent component)
        {
            return new Vector2(Width(component), Height(component));
        }

        public static implicit operator Dimensions(Point p)
        {
            return new SimpleDimensions(p.X, p.Y);
        }

        public static implicit operator Dimensions(Vector2 v)
        {
            return new SimpleDimensions((int)v.X, (int)v.Y);
        }
    }
}
