
namespace ConOOD_RpgGame
{
    public abstract class Item
    {
        public string Name { get; }
        public string Description { get; }
        public char Symbol { get; }
        protected Item(string name, char symbol, string description) //cons
        {
            Name = name;
            Description = description;
            Symbol = symbol;
        }
        public virtual string GetDisplayText()
        {
            return Name;
        }
        public virtual string GetDetailedInfo() {
            return $"{Name}-{Description}";
        }
        public virtual bool PickUp(Player player, out string message)
        {
            player.AddToInventory(this);
            message = $"{Name} added to inventory.";
            return true;
        }
        public virtual bool TryEquip(Player player, HandSide handside, out string message)
        {
            message = $"{Name} cannot be equipped.";
            return false;
        }
    }
}
