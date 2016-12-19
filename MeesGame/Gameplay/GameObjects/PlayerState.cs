using Microsoft.Xna.Framework;
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

    public class PlayerState
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
