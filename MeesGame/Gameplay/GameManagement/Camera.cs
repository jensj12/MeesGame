using Microsoft.Xna.Framework;

namespace MeesGame
{
    class Camera : GameObjectList
    {
        GameObject objectToFollow;
        float tolerance;

        /// <summary>
        /// Creates a camara object which tracks the specified object
        /// </summary>
        /// <param name="objectToFollow">The camera will be centered above this object</param>
        /// <param name="tolerace">Constant defining the tracking speed of the camera. the higher the slowe it tracks. Below 1 obviously causes graphical glitches (it always overshoots)</param>

        public Camera(GameObject objectToFollow = null, float tolerance = 8f) : base(0, "camera")
        {
            this.objectToFollow = objectToFollow;
            this.tolerance = tolerance;
        }

        /// <summary>
        /// Centers the camera above the object it follows
        /// Is updated continuously
        /// </summary>
        public void UpdateCamera()
        {
            if (objectToFollow == null)
            {
                position = Vector2.Zero;
                return;
            }
            Rectangle rect = objectToFollow.BoundingBox;
            rect.Location = objectToFollow.Position.ToPoint();
            float preferredX = rect.Center.X - GameEnvironment.Screen.X / 2;
            float preferredY = rect.Center.Y - GameEnvironment.Screen.Y / 2;
            TileField tiles = Find("tiles") as TileField;
            preferredX = -MathHelper.Clamp(preferredX, 0, tiles.CellWidth * tiles.Columns - GameEnvironment.Screen.X);
            preferredY = -MathHelper.Clamp(preferredY, 0, tiles.CellHeight * tiles.Rows - GameEnvironment.Screen.Y);
            position.X += InterpolerisationToPoint(preferredX, position.X);
            position.Y += InterpolerisationToPoint(preferredY, position.Y);

        }

        private float InterpolerisationToPoint(float desiredLocation, float currentLocation)
        {
            if (desiredLocation - currentLocation != 0)
            {
                return (desiredLocation - currentLocation) / tolerance;
            }
            return 0;
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
