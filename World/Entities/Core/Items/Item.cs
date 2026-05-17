using ConOOD_RpgGame.World.Enums;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public abstract class Item
    {
        public string Name { get; }
        public string Description { get; }
        public char Symbol { get; }

        protected Item(string name, char symbol, string description)
        {
            Name = name;
            Description = description;
            Symbol = symbol;
        }

        public virtual string GetDisplayText()
        {
            return Name;
        }

        public virtual string GetDetailedInfo()
        {
            return $"{Name} - {Description}";
        }

        public virtual bool PickUp(Player player, out string message)
        {
            player.AddToInventory(this);
            message = $"{Name} added to inventory.";
            return true;
        }

        public virtual bool TryEquip(Player player, HandSide handSide, out string message)
        {
            message = $"{Name} cannot be equipped.";
            return false;
        }

        public virtual int GetStrengthBonus()
        {
            return 0;
        }

        public virtual int GetDexterityBonus()
        {
            return 0;
        }

        public virtual int GetHealthBonus()
        {
            return 0;
        }

        public virtual int GetLuckBonus()
        {
            return 0;
        }

        public virtual int GetAggressionBonus()
        {
            return 0;
        }

        public virtual int GetWisdomBonus()
        {
            return 0;
        }

        public virtual int GetDamageBonus()
        {
            return 0;
        }

        public virtual void Accept(IAttackVisitor visitor, Player player)
        {
            visitor.VisitItem(this, player);
        }
        public virtual bool CanBeEquipped()
        {
            return false;
        }

        public virtual bool IsTwoHandedWeapon()
        {
            return false;
        }
        public virtual int GetNoiseRange()
        {
            return 0;
        }

    }
}