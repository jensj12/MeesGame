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

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return type.Equals((obj as InventoryItem).type);
        }
    }

    class InventoryKey : InventoryItem
    {
        public InventoryKey()
        {
            type = InventoryItemType.Key;
        }
    }

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
