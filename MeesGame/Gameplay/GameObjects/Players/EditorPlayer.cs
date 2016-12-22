using Microsoft.Xna.Framework;

namespace MeesGame
{
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
