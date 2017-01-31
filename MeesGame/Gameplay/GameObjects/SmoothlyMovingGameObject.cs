using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    public interface IDiscreteField
    {
        Vector2 GetAnchorPosition(Point location);
        Vector2 CellDimensions { get; }
        bool OutOfTileField(Point location);
    }

    /// <summary>
    /// Game object that exists at discrete positions but animates smoothly to its new position when moving
    /// </summary>
    public class SmoothlyMovingGameObject : DirectionalGameObject
    {
        /// <summary>
        /// Event called when the location of the object changes
        /// </summary>
        public event GameObjectEventHandler LocationChanged;

        /// <summary>
        /// Event called when the object stops moving
        /// </summary>
        public event GameObjectEventHandler StoppedMoving;

        /// <summary>
        /// When the current movement started
        /// </summary>
        private TimeSpan startOfCurrentMovement;

        /// <summary>
        /// Whether we just started moving
        /// </summary>
        private bool justStartedMoving = false;

        /// <summary>
        /// The field that this object is moving on
        /// </summary>
        protected IDiscreteField field;

        public Direction LastDirection
        {
            get; private set;
        }

        public SmoothlyMovingGameObject(IDiscreteField field, TimeSpan travelTime, string assetName, int layer = 0, string id = "", int sheetIndex = 0) : base(assetName, layer, id, sheetIndex)
        {
            this.field = field;
            TravelTime = travelTime;
        }

        /// <summary>
        /// The time it takes to travel one unit
        /// </summary>
        public TimeSpan TravelTime
        {
            get;
        }

        private Point location;
        /// <summary>
        /// The location on the field
        /// </summary>
        public Point Location
        {
            get
            {
                return location;
            }

            private set
            {
                location = value;
                LocationChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Go to the center of the cell in the field and stop moving
        /// </summary>
        private void JumpToAnchorPosition()
        {
            StopMoving();
            position = field.GetAnchorPosition(Location);
        }

        /// <summary>
        /// Move the object in the specified direction. The object's location will be updated immediately, 
        /// but its position will transition smoothly.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSmoothly(Direction direction)
        {
            Direction = direction;
            JumpToAnchorPosition();
            LastDirection = direction;
            Location += direction.ToPoint();
            Vector2 translation = Vector2.Multiply(field.CellDimensions, direction.ToVector2());
            velocity = Vector2.Divide(translation, (float)TravelTime.TotalSeconds);
            justStartedMoving = true;
        }

        /// <summary>
        /// Teleport this object directly to the new location. No smooth movement will be shown.
        /// </summary>
        /// <param name="newLocation"></param>
        public void Teleport(Point newLocation)
        {
            Location = newLocation;
            JumpToAnchorPosition();
        }

        /// <summary>
        /// Whether the object is currently moving
        /// </summary>
        public bool IsMoving
        {
            get; private set;
        }

        /// <summary>
        /// Stop the object from moving
        /// </summary>
        protected void StopMoving()
        {
            velocity = Vector2.Zero;
            IsMoving = false;
            StoppedMoving?.Invoke(this);
        }

        private bool isMovementComplete(GameTime gameTime)
        {
            return gameTime.TotalGameTime - startOfCurrentMovement >= TravelTime;
        }

        public override void Update(GameTime gameTime)
        {
            if (justStartedMoving)
            {
                startOfCurrentMovement = gameTime.TotalGameTime;
                justStartedMoving = false;
                IsMoving = true;
            }
            if (IsMoving && isMovementComplete(gameTime))
                JumpToAnchorPosition();
            base.Update(gameTime);
        }
    }
}
