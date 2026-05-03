using ConOOD_RpgGame.World.Enums;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public abstract class Weapon : Item
    {
        public int Damage { get; }
        public bool IsTwoHanded { get; }
        protected abstract string WeaponTypeName { get; }
        protected Weapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description)
        {
            Damage = damage;
            IsTwoHanded = isTwoHanded;
        }

        public override string GetDisplayText()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";
            return $"{Name} [{WeaponTypeName}] (DMG: {Damage + GetDamageBonus()}, {handText})";
        }

        public override string GetDetailedInfo()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";
            return $"{Name} [{WeaponTypeName}] - Damage: {Damage + GetDamageBonus()}, {handText}, {Description}";
        }
        public override bool TryEquip(Player player, HandSide handSide, out string message)
        {
            return player.EquipItemDirectly(this, handSide, out message);
        }

        public int GetFinalBaseDamage()
        {
            return Damage + GetDamageBonus();
        }

        public abstract override void Accept(IAttackVisitor visitor, Player player);
        public int GetBaseDamage()
        {
            return Damage;
        }
        public override bool CanBeEquipped()
        {
            return true;
        }

        public override bool IsTwoHandedWeapon()
        {
            return IsTwoHanded;
        }
    }
}