using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.GameObjects
{
    class InventoryKey : InventoryItem
    {
        public InventoryKey()
        {
            type = InventoryItemType.Key;
        }
    }
}
