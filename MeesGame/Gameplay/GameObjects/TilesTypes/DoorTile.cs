using Microsoft.Xna.Framework;
using System.ComponentModel;
using static MeesGame.PlayerAction;

namespace MeesGame
{
    class DoorTile : WallTile
    {
        protected bool isHorizontal = true;
        protected KeyColor doorColor;

        public Color Background { get; set; }

        public DoorTile(int layer = 0, string id = "") : base(TileType.Door, layer, id)
        {
            secondarySpriteColor = doorColor.ToColor();
        }

        [Editor]
        public Color SecondarySpriteColor
        {
            get
            {
                return base.secondarySpriteColor;
            }
            set
            {
                base.secondarySpriteColor = value;
                doorColor = KeyColorExtensions.FromColor(value);
                TileData tileData = Data;
                tileData.AdditionalInfo = (int)doorColor;
                Data = tileData;
            }
        }

        public override bool CanPlayerMoveHere(ITileFieldPlayer player)
        {
            // A player is allowed to move onto a horizontalDoor from the tile above or below it.
            // A player is allowed to move onto a verticalDoor from the Tile to the left or to the right of it.
            if ((isHorizontal && player.Location.X == Location.X) || (!isHorizontal && player.Location.Y == Location.Y))
            {
                //A player is allowed to move onto a door tile if the door is open.
                if (CanMoveOntoDoor(player))
                    return true;
                else
                    return false;
            }
            else return false;
        }

        public override bool IsActionForbiddenFromHere(ITileFieldPlayer player, PlayerAction action)
        {
            //A player can only move back to where he came from or into the opposite direction
            if (action == EAST || action == WEST)
            {
                if (player.LastAction == EAST || player.LastAction == WEST)
                    return false;
                else
                    return true;
            }
            else if (action == NORTH || action == SOUTH)
            {
                if (player.LastAction == NORTH || player.LastAction == SOUTH)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        public bool CanMoveOntoDoor(ITileFieldPlayer player)
        {
            //A player is only allowed to open a door if he has the right key.
            if (player.HasItem(doorColor.ToInventeryItemType()))
            {
                return true;
            }
            else
                return false;
        }

        public override void EnterTile(ITileFieldPlayer player)
        {
            GameEnvironment.AssetManager.PlaySound("open_door");
            base.EnterTile(player);
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
            int useHorizontal = 0;
            int x = Location.X;
            int y = Location.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile) useHorizontal -= 1;
            if (tileField.GetTile(x + 1, y) is WallTile) useHorizontal += 1;
            if (tileField.GetTile(x, y + 1) is WallTile) useHorizontal -= 1;
            if (tileField.GetTile(x - 1, y) is WallTile) useHorizontal += 1;
            if (useHorizontal <= -1)
            {
                sprite = new SpriteSheet("VerticalDoor");
                secondarySprite = new SpriteSheet("VerticalDoorOverlay");
                isHorizontal = false;
            }
            else
            {
                sprite = new SpriteSheet("HorizontalDoor");
                secondarySprite = new SpriteSheet("HorizontalDoorOverlay");
            }
        }

        public override void UpdateToAdditionalInfo()
        {
            doorColor = (KeyColor)Data.AdditionalInfo;
            secondarySpriteColor = doorColor.ToColor();
        }
    }
}
