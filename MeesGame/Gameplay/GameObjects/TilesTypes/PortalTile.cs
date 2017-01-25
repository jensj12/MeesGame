using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace MeesGame
{
    class PortalTile : FloorTile
    {
        protected int portalIndex;
        protected Point destination;

        public PortalTile(int layer = 0, string id = "") : base(TileType.Portal, layer, id)
        {
        }

        [Editor]
        public int PortalIndex
        {
            get
            {
                return portalIndex;
            }
            set
            {
                portalIndex = value;
                TileData tileData = Data;
                tileData.AdditionalInfo = portalIndex;
                Data = tileData;
            }
        }

        public Point Destination
        {
            get
            {
                return destination;
            }
        }

        public override bool IsActionForbiddenFromHere(ITileFieldPlayer player, PlayerAction action)
        {
            if (action == PlayerAction.SPECIAL)
                // Special actions are allowed on portalTiles.
                return false;

            return base.IsActionForbiddenFromHere(player, action);
        }

        /// <summary>
        /// Teleport to the other portal in case of special action.
        /// </summary>
        public override void PerformAction(ITileFieldPlayer player, PlayerAction action)
        {
            base.PerformAction(player, action);
            if (action == PlayerAction.SPECIAL)
            {
                player.Teleport(destination);
            }
        }

        /// <summary>
        /// Go to the other portal when a special action is performed.
        /// </summary>
        protected override Point GetLocationAfterAction(PlayerAction action)
        {
            if (action == PlayerAction.SPECIAL)
                return destination;
            return base.GetLocationAfterAction(action);
        }

        public override void UpdateToAdditionalInfo()
        {
            portalIndex = Data.AdditionalInfo;
        }

        /// <summary>
        /// Sets the destination of the portal.
        /// </summary>
        public override void UpdatePortals()
        {
            int width = TileField.Objects.GetLength(0);
            int Height = TileField.Objects.GetLength(1);

            int x = Location.X + 1;
            int y = Location.Y;

            bool found = false;

            while (!found)
            {
                y %= Height;
                while (x < width)
                {
                    Tile target = (Tile)TileField.Objects[x, y];
                    if(target.TileType == TileType.Portal)
                    {
                        destination = target.Location;
                        found = true;
                        break;
                    }
                    if (target == this)
                    {
                        found = true;
                        break;
                    }
                    x++;
                }
                x = 0;
                y++;
            }

        }
    }
}
