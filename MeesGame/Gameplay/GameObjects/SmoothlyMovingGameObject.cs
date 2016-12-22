using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    /// <summary>
    /// Game object that exists at discrete positions but animates smoothly to its new position when moving
    /// </summary>
    public class SmoothlyMovingGameObject : RotatableGameObject
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

        public SmoothlyMovingGameObject(Dictionary<Direction,SpriteSheet> sprites, IDiscreteField field, TimeSpan travelTime, string assetName, int layer = 0, string id = "", int sheetIndex = 0) : base(sprites, assetName, layer, id, sheetIndex)
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
            FacingDirection = direction;
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
        protected virtual void StopMoving()
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
