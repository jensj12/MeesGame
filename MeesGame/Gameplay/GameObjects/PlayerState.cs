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
        public Inventory Inventory
        {
            get;
        }

        public int Score
        {
            get; set;
        }

        public SmoothlyMovingGameObject Character
        {
            get;
        }

        public PlayerState(SmoothlyMovingGameObject character)
        {
            Character = character;
            Inventory = new Inventory();
        }
    }
}
