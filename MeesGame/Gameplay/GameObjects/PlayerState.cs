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

    class InventoryKey : InventoryItem
    {
        public InventoryKey(InventoryItemType iit = InventoryItemType.Key)
        {

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
