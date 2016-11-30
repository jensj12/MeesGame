using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIList : UIObject
    {
        private int elementsDistance;
        public int ElementsDistance
        {
            get { return elementsDistance;}
        }
        protected int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
        }
    
        public UIList(Vector2 location, Vector2 dimensions, UIObject parent, int elementsDistance = 10) : base(location, dimensions, parent, true)
        {
        }
    }
}
