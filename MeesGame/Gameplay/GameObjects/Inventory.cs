using System.Collections.Generic;

namespace MeesGame
{
    public class Inventory
    {
        public ISet<InventoryItem> Items
        {
            get;
        }

        public Inventory()
        {
            Items = new HashSet<InventoryItem>();
        }
    }
}
