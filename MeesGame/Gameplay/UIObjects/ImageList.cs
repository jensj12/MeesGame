using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MeesGame.Button;

namespace MeesGame.Gameplay.UIObjects
{
    class ImageList : UIList
    {
        public ImageList(Vector2 location, Vector2 dimensions, UIObject parent, ClickEventHandler clickEventHandler, int elementsDistance = 10) : base(location, dimensions, parent, elementsDistance)
        {
        }

        public void AddImageButton(ListButton[] buttons) {
            foreach (ListButton button in buttons)
                children.Add(button);
            scrollBar.ChangeTotalElementsSize();
        }
    }
}
