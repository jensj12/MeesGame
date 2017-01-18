namespace MeesGame
{
    class InventoryKey : InventoryItem
    {
        public InventoryKey(KeyColor color)
        {
            type = color.ToInventeryItemType();
        }
    }
}
