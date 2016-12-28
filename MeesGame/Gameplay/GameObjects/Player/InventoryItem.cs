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
                    return Tile.GetAssetNamesFromTileType(TileType.Key)[1];
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
}
