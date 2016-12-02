using Microsoft.Xna.Framework;

namespace MeesGame
{
    class Camera : GameObjectList
    {
        GameObject objectToFollow;

        /// <summary>
        /// Creates a camara object which tracks the specified object
        /// </summary>
        /// <param name="objectToFollow">The camera will be centered above this object</param>
        public Camera(GameObject objectToFollow = null) : base(0, "camera")
        {
            this.objectToFollow = objectToFollow;
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
            float preferredX = rect.Center.X - GameEnvironment.Screen.X / 2;
            float preferredY = rect.Center.Y - GameEnvironment.Screen.Y / 2;
            TileField tiles = Find("tiles") as TileField;
            position.X = -MathHelper.Clamp(preferredX, 0, tiles.CellWidth * tiles.Columns - GameEnvironment.Screen.X);
            position.Y = -MathHelper.Clamp(preferredY, 0, tiles.CellHeight * tiles.Rows - GameEnvironment.Screen.Y);
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
