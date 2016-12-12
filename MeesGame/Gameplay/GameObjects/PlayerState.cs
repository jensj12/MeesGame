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
        public Inventory Inventory{
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
