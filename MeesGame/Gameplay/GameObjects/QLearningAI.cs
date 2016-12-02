using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MeesGame.CharacterAction;


namespace MeesGame.Gameplay.GameObjects
{
    class QLearningAI : Character
    {
        public QLearningAI(Level level, TileField tileField, Point location, int layer = 0, string id = "", int score = 0) : base(level, tileField, location, layer, id, score)
        {
            this.score = score;
            this.level = level;
            this.location = location;
            this.position = tileField.GetAnchorPosition(location);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(Keys.Q))
            {
                //begin met leren
            }
            else if (inputHelper.IsKeyDown(Keys.W))
            {
                //stop met leren
            }
            else if (inputHelper.IsKeyDown(Keys.E))
            {
                //doe het voor het echie
            }
        }
    }
}
