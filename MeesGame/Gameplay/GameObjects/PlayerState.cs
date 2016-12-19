using Microsoft.Xna.Framework;
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

    class PlayerState
    {
        public Inventory Inventory
        {
            get;
        }

        public int Score
        {
            get; set;
        }

        public Point Location
        {
            get; set;
        }

        public PlayerState()
        {
            Inventory = new Inventory();
        }
    }
}