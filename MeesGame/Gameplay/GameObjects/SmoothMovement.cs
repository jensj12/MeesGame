using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }

    static class DirectionExtensions
    {
        /// <summary>
        /// Converts the direction into a point with absolute value 1
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>A point with absolute value 1 in the direction relative to the origin</returns>
        public static Point ToPoint(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return new Point(0, -1);
                case Direction.EAST:
                    return new Point(1, 0);
                case Direction.SOUTH:
                    return new Point(0, 1);
                case Direction.WEST:
                    return new Point(-1, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts the direction into a vector with absolute value 1
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>A vector with absolute value 1 in the direction relative to the origin</returns>
        public static Vector2 ToVector2(this Direction direction)
        {
            Point p = direction.ToPoint();
            return new Vector2(p.X, p.Y);
        }

        public static bool IsDirection(this PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.NORTH:
                case PlayerAction.EAST:
                case PlayerAction.SOUTH:
                case PlayerAction.WEST:
                    return true;
                default:
                    return false;
            }
        }

        public static Direction ToDirection(this PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.NORTH:
                    return Direction.NORTH;
                case PlayerAction.EAST:
                    return Direction.EAST;
                case PlayerAction.SOUTH:
                    return Direction.SOUTH;
                case PlayerAction.WEST:
                    return Direction.WEST;
                default:
                    throw new PlayerActionNotAllowedException();
            }
        }
    }

    interface IDiscreteField
    {
        Vector2 GetAnchorPosition(Point location);
        Vector2 CellDimensions { get; }
    }

    /// <summary>
    /// Game object that exists at discrete positions but animates smoothly to its new position when moving
    /// </summary>
    class SmoothlyMovingGameObject : SpriteGameObject
    {
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
        private IDiscreteField field;

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
                OnLocationChanged();
            }
        }

        protected virtual void OnLocationChanged()
        {

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
            JumpToAnchorPosition();
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
        }

        public override void Update(GameTime gameTime)
        {
            if (justStartedMoving)
            {
                startOfCurrentMovement = gameTime.TotalGameTime;
                justStartedMoving = false;
                IsMoving = true;
            }
            if (IsMoving)
            {
                bool isMovementComplete = gameTime.TotalGameTime - startOfCurrentMovement >= TravelTime;
                if (isMovementComplete)
                {
                    JumpToAnchorPosition();
                }
            }
            base.Update(gameTime);
        }
    }
}
