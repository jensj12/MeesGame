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
        public override Point GetLocationAfterAction(PlayerAction action)
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
            foreach (Tile tile in this.TileField.Objects)
            {
                if (tile.Data.AdditionalInfo == portalIndex && tile != this && tile.TileType == TileType.Portal)
                {
                    destination = tile.Location;
                }
            }
        }
    }
}
