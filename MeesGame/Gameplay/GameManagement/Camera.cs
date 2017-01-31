using Microsoft.Xna.Framework;

namespace MeesGame
{
    public class Camera : GameObjectList
    {
        GameObject objectToFollow;

        /// <summary>
        /// Visible space the camera should render to.
        /// </summary>
        private Point? screenSize;

        /// <summary>
        /// Creates a camera object which tracks the specified object
        /// </summary>
        /// <param name="screenSize">Visible space, corners of the map should end here. If left to null, uses GameEnvironment.Screen</param>
        /// <param name="objectToFollow">object, usually the player, the camera should track</param>
        public Camera(Point? screenSize = null, GameObject objectToFollow = null) : base(0, "camera")
        {
            this.objectToFollow = objectToFollow;
            this.screenSize = screenSize;
        }

        /// <summary>
        /// Centers the camera above the object it follows
        /// </summary>
        public void UpdateCamera()
        {
            if (objectToFollow == null)
            {
                position = Vector2.Zero;
                return;
            }
            Rectangle rect = objectToFollow.BoundingBox;
            float preferredX = rect.Center.X - ScreenSize.X / 2;
            float preferredY = rect.Center.Y - ScreenSize.Y / 2;
            TileField tiles = (TileField)Find("tiles");
            position.X = -MathHelper.Clamp(preferredX, 0, tiles.CellWidth * tiles.Columns - ScreenSize.X);
            position.Y = -MathHelper.Clamp(preferredY, 0, tiles.CellHeight * tiles.Rows - ScreenSize.Y);
        }

        public Point ScreenSize
        {
            get
            {
                return screenSize ?? GameEnvironment.Screen;
            }
        }

        /// <summary>
        /// Sets the camera's screensize.
        /// </summary>
        /// <param name="screenSize">If null, uses the GameEnvironment.Screen</param>
        public void SetScreenSize(Point? screenSize)
        {
            this.screenSize = screenSize;
        }

        public void ResetCamera()
        {
            position = Vector2.Zero;
        }
    }
}
