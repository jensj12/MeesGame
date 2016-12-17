using Microsoft.Xna.Framework;

namespace MeesGame
{
    class Camera : GameObjectList
    {
        GameObject objectToFollow;

        //we need to be able to create custom screen size in order to resize the screen when editing
        Point screenSize;

        /// <summary>
        /// Creates a camara object which tracks the specified object
        /// </summary>
        /// <param name="screenSize">Visible space, corners of the map should end here</param>
        /// <param name="objectToFollow">object, usually the player, the camera should track</param>
        public Camera(Point screenSize, GameObject objectToFollow = null) : base(0, "camera")
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
            float preferredX = rect.Center.X - screenSize.X / 2;
            float preferredY = rect.Center.Y - screenSize.Y / 2;
            TileField tiles = (TileField)Find("tiles");
            position.X = -MathHelper.Clamp(preferredX, 0, tiles.CellWidth * tiles.Columns - screenSize.X);
            position.Y = -MathHelper.Clamp(preferredY, 0, tiles.CellHeight * tiles.Rows - screenSize.Y);
        }

        public void ResetCamera()
        {
            position = Vector2.Zero;
        }

        public GameObject ObjectToFollow
        {
            get { return objectToFollow; }
            set { objectToFollow = value; }
        }
    }
}
