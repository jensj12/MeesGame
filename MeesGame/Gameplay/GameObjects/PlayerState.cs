using System.Collections.Generic;

namespace MeesGame
{
    public enum InventoryItemType
    {
        Key
    }

    public abstract class InventoryItem
    {
        public InventoryItemType type
        {
            get; set;
        }

        public static string inventoryItemAsset(InventoryItemType ii)
        {
            switch (ii)
            {
                case InventoryItemType.Key:
                    return KeyTile.GetDefaultAssetNames()[1];
            }
            return null;
        }
    }

    class InventoryKey : InventoryItem
    {
        public InventoryKey(InventoryItemType iit = InventoryItemType.Key)
        {
        }
    }

    public class Inventory
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
