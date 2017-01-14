namespace MeesGame
{
    public enum InventoryItemType
    {
        KeyBlue,
        KeyCyan,
        KeyGreen,
        KeyMagenta,
        KeyRed,
        KeyYellow
    }

    public abstract class InventoryItem
    {
        public InventoryItemType type
        {
            get; set;
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
