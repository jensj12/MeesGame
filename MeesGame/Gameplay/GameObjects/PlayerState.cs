using System.Collections.Generic;

namespace MeesGame
{
    enum InventoryItemType
    {
        Key
    }

    abstract class InventoryItem
    {
        public InventoryItemType type
        {
            get; set;
        }

    }

    class Inventory
    {
        public List<InventoryItem> Items
        {
            get;
        }

        public Inventory()
        {
            Items = new List<InventoryItem>();
        }
    }
}
