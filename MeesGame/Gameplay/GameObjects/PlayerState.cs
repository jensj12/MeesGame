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

    class InventoryKey : InventoryItem
    {
        public InventoryKey(InventoryItemType iit = InventoryItemType.Key)
        {

        }
    }


    class Inventory
    {
        public delegate void InventoryChangedEventHandler();
        public event InventoryChangedEventHandler OnInventoryChanged;

        public List<InventoryItem> Items
        {
            get;
        }

        public Inventory()
        {
            Items = new List<InventoryItem>();
        }

        private void InventoryChanged()
        {
            OnInventoryChanged?.Invoke();
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
