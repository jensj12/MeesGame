using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    public class Player : SmoothlyMovingGameObject
    {
        public delegate void PlayerActionHandler(PlayerAction action);
        public delegate void PlayerWinHandler(Player player);
        public event PlayerWinHandler PlayerWin;
        public event PlayerActionHandler PlayerAction;

        public Player(Level level, Point location, int layer = 0, string id = "", int score = 0) : base(level.Tiles,level.TimeBetweenActions,"playerdown@4", layer, id)
        {
            Score = score;
            Level = level;
            Teleport(location);
            Inventory = new Inventory();
            Level.Tiles.RevealArea(location);
        }

        /// <summary>
        /// Current score of the player
        /// </summary>
        public int Score
        {
            get; set;
        }

        /// <summary>
        /// The level that is played on
        /// </summary>
        public Level Level
        {
            get;
        }

        /// <summary>
        /// The last action performed by this player
        /// </summary>
        public PlayerAction LastAction
        {
            get; private set;
        }

        /// <summary>
        /// A list of all possible actions the player can take at the current stage of the game
        /// </summary>
        public ICollection<PlayerAction> Actions
        {
            get
            {
                List<PlayerAction> actions = new List<PlayerAction>();
                foreach (PlayerAction action in Enum.GetValues(typeof(PlayerAction)))
                {
                    if (CanPerformAction(action)) actions.Add(action);
                }
                return actions;
            }
        }

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();
            CurrentTile.EnterTile(this);
        }

        protected override void StopMoving()
        {
            base.StopMoving();
            CurrentTile.EnterCenterOfTile(this);
        }

        /// <summary>
        /// Checks if a player can perform the specified action
        /// </summary>
        /// <param name="action">The action to check</param>
        /// <returns>true, if the player can perform the action. false otherwise.</returns>
        public virtual bool CanPerformAction(PlayerAction action)
        {
            if (CurrentTile.IsActionForbiddenFromHere(this, action)) return false;

            Point newLocation = CurrentTile.GetLocationAfterAction(action);
            return Level.Tiles.GetTile(newLocation).CanPlayerMoveHere(this);
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        /// <param name="action">The PlayerAction to perform</param>
        public void PerformAction(PlayerAction action)
        {
            if (!CanPerformAction(action)) throw new PlayerActionNotAllowedException();
            LastAction = action;

            CurrentTile.PerformAction(this, action);
            InventoryItem item = CurrentTile.GetItem();
            if (item != null)
                Inventory.Items.Add(item);

            this.Level.Tiles.RevealArea(Location);
            CurrentTile.IsVisited = true;

            PlayerActionEvent(action);
        }
        

        /// <summary>
        /// The Tile the player is currently on
        /// </summary>
        public Tile CurrentTile
        {
            get
            {
                return Level.Tiles.GetTile(Location);
            }
        }

        public bool HasKey()
        {
            foreach (InventoryItem item in (Inventory.Items))
                if (item.type == InventoryItemType.Key)
                    return true;
            return false;
        }

        public void Win()
        {
            PlayerWin?.Invoke(this);
        }

        // TODO: make the player lose instead of win
        public void Lose()
        {
            PlayerWin?.Invoke(this);
        }

        /// <summary>
        /// List of actions that can be used to move players
        /// </summary>
        public static readonly PlayerAction[] MOVEMENT_ACTIONS = new PlayerAction[] { NORTH, WEST, SOUTH, EAST };

        public void PlayerActionEvent(PlayerAction action)
        {
            PlayerAction?.Invoke(action);
        }

        public Inventory Inventory
        {
            get;
        }
    }

    /// <summary>
    /// An exception that is thrown when a Player tries to perform an Action that is not allowed.
    /// </summary>
    class PlayerActionNotAllowedException : Exception
    {
    }

    class TimedPlayer : Player, IPlayer
    {
        /// <summary>
        /// The time that the last action was performed
        /// </summary>
        protected TimeSpan lastActionTime;

        protected TimeSpan lastAnimationTime;

        public TimedPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animate(gameTime);
            if (!IsMoving)
            {
                if (CanPerformAction(NextAction) && NextAction != NONE)
                {
                    PerformAction(NextAction);

                    if (NextAction == MeesGame.PlayerAction.NORTH)
                    {
                        this.sprite = new SpriteSheet("playerup@4");
                    }
                    else if (NextAction == MeesGame.PlayerAction.SOUTH)
                    {
                        this.sprite = new SpriteSheet("playerdown@4");
                    }
                    else if (NextAction == MeesGame.PlayerAction.EAST)
                    {
                        this.sprite = new SpriteSheet("playerright@4");
                    }
                    else if (NextAction == MeesGame.PlayerAction.WEST)
                    {
                        this.sprite = new SpriteSheet("playerleft@4");
                    }

                    lastActionTime = gameTime.TotalGameTime;
                    NextAction = NONE;
                }
                else
                {
                    this.Sprite.SheetIndex = 0;
                }
            }
        }

        /// <summary>
        /// Calculates a Vector for the animation velocity of the player so that the player removes smoothly between tiles
        /// </summary>
        /// <param name="action">The action that should be animated</param>
        /// <returns></returns>
        private Vector2 CalculateVelocityVector(PlayerAction action)
        {
            Vector2 direction = GetDirectionVector(action);
            float speed = Speed;
            return new Vector2(direction.X * speed, direction.Y * speed);
        }

        /// <summary>
        /// Gets a direction vector based on the action of the player
        /// </summary>
        /// <param name="action">The action that the direction vector should represent</param>
        /// <returns>A Vector with absolute value of 1 in the direction of movement associated with the action</returns>
        private Vector2 GetDirectionVector(PlayerAction action)
        {
            switch (action)
            {
                case NORTH:
                    return new Vector2(0, -1);

                case EAST:
                    return new Vector2(1, 0);

                case SOUTH:
                    return new Vector2(0, 1);

                case WEST:
                    return new Vector2(-1, 0);
            }
            return new Vector2(0, 0);
        }

        public void animate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastAnimationTime.TotalMilliseconds >= this.Level.TimeBetweenActions.TotalMilliseconds / 4)
            {
                this.Sprite.SheetIndex = (this.Sprite.SheetIndex + 1) % 4;
                lastAnimationTime = gameTime.TotalGameTime;
            }
        }

        /// <summary>
        /// The speed of the player for animation in px per seconds
        /// </summary>
        private float Speed
        {
            get
            {
                return (float)(Level.Tiles.CellHeight / Level.TimeBetweenActions.TotalSeconds);
            }
        }

        /// <summary>
        /// The action that will be performed as soon as another action can be performed
        /// </summary>
        public PlayerAction NextAction
        {
            get; set;
        } = NONE;

        public ITileField TileField
        {
            get
            {
                return Level.Tiles;
            }
        }
    }

    class HumanPlayer : TimedPlayer
    {
        public HumanPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.W) || inputHelper.IsKeyDown(Keys.Up))
            {
                NextAction = NORTH;
            }
            else if (inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right))
            {
                NextAction = EAST;
            }
            else if (inputHelper.IsKeyDown(Keys.S) || inputHelper.IsKeyDown(Keys.Down))
            {
                NextAction = SOUTH;
            }
            else if (inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left))
            {
                NextAction = WEST;
            }
            else
            {
                NextAction = NONE;
            }
        }
    }

    /// <summary>
    /// A player that can move over obstacles
    /// </summary>
    class EditorPlayer : HumanPlayer
    {
        public EditorPlayer(Level level, Point location, int score = 0) : base(level, location, score)
        {
        }

        /// <summary>
        /// An editorplayer can move everywhere on the map. He only cannot move out of the maps bounds
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override bool CanPerformAction(PlayerAction action)
        {
            //An editorplayer can not do special moves
            if (action == MeesGame.PlayerAction.SPECIAL) return false;

            Point newLocation = CurrentTile.GetLocationAfterAction(action);
            //If the editorplayer may only not move out of the tilefield
            return !Level.Tiles.OutOfTileField(newLocation.X, newLocation.Y);
        }
    }
}
