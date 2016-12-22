using Microsoft.Xna.Framework;

namespace MeesGame
{
    public interface IDiscreteField
    {
        Vector2 GetAnchorPosition(Point location);
        Vector2 CellDimensions { get; }
    }
}
