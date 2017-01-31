using Microsoft.Xna.Framework;
using System.ComponentModel;
using static MeesGame.PlayerAction;
using System;

namespace MeesGame
{
    class DoorTile : WallTile
    {
        protected bool isHorizontal = true;
        protected KeyColor doorColor;

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
                doorColor = value.ToKeyColor();
                TileData tileData = Data;
                tileData.AdditionalInfo = (int)doorColor;
                Data = tileData;
            }
        }

        private bool IsComingFromAllowedDirection(ITileFieldPlayer player)
        {
            if (isHorizontal)
                // A player is allowed to move onto a horizontalDoor from the tile above or below it.
                return player.Location.X == Location.X;
            else
                // A player is allowed to move onto a verticalDoor from the Tile to the left or to the right of it.
                return player.Location.Y == Location.Y;
        }

        public override bool CanPlayerMoveHere(ITileFieldPlayer player)
        {
            //A player is allowed to move onto a door tile if he comes from the correct direction and 
            //the door is open.
            return IsComingFromAllowedDirection(player) && IsDoorOpenFor(player);
        }

        public override bool IsActionForbiddenFromHere(ITileFieldPlayer player, PlayerAction action)
        {
            // only directional actions are allowed
            if (!action.IsDirection()) return true;

            //A player can move only in the directions that the door allows
            if (isHorizontal)
                //on horizontal doors only vertical actions are allowed
                return !action.ToDirection().IsVertical() || base.IsActionForbiddenFromHere(player, action);
            else
                //on vertical doors only horizontal actions are allowed
                return !action.ToDirection().IsHorizontal() || base.IsActionForbiddenFromHere(player, action);
        }

        public bool IsDoorOpenFor(ITileFieldPlayer player)
        {
            //A player is only allowed to open a door if he has the right key.
            return player.HasItem(doorColor.ToInventeryItemType());
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

        public KeyColor DoorColor
        {
            get { return doorColor; }
        }
    }
}
