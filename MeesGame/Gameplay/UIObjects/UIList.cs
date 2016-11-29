using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.UIObjects
{
    abstract class UIList : UIObject
    {
        private UIObjectList objectlist;
        private int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
        }
    }
}
