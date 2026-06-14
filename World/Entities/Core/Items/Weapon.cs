using ConOOD_RpgGame.World.Enums;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public abstract class Weapon : Item
    {
        public int Damage { get; }
        public bool IsTwoHanded { get; }

        public abstract int NoiseRange { get; }

        protected abstract string WeaponTypeName { get; }

        private readonly List<Item> slots;
        private readonly int slotCount;

        protected Weapon(
            string name,
            char symbol,
            string description,
            int damage,
            bool isTwoHanded,
            int slotCount = 0)
            : base(name, symbol, description)
        {
            Damage = damage;
            IsTwoHanded = isTwoHanded;
            this.slotCount = slotCount;
            slots = new List<Item>();
        }

        public bool AddToSlot(Item item, out string message)
        {
            if (item == null)
            {
                message = "Item is empty.";
                return false;
            }

            if (!item.CanBePlacedInSlot())
            {
                message = $"{item.Name} cannot be placed in a weapon slot.";
                return false;
            }

            if (slots.Count >= slotCount)
            {
                message = $"{Name} has no empty slots.";
                return false;
            }

            slots.Add(item);
            message = $"{item.Name} placed into {Name}.";
            return true;
        }

        public override string GetDisplayText()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";
            return $"{Name} [{WeaponTypeName}] (DMG: {Damage + GetDamageBonus()}, {handText}, Slots:{slots.Count}/{slotCount})";
        }

        public override string GetDetailedInfo()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";
            return $"{Name} [{WeaponTypeName}] - Damage: {Damage + GetDamageBonus()}, {handText}, Slots:{slots.Count}/{slotCount}, {Description}";
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

        public override int GetNoiseRange()
        {
            return NoiseRange;
        }

        public override int GetStrengthBonus()
        {
            int total = 0;

            foreach (Item item in slots)
            {
                total += item.GetStrengthBonus();
            }

            return total;
        }

        public override int GetDexterityBonus()
        {
            int total = 0;

            foreach (Item item in slots)
            {
                total += item.GetDexterityBonus();
            }

            return total;
        }

        public override int GetLuckBonus()
        {
            int total = 0;

            foreach (Item item in slots)
            {
                total += item.GetLuckBonus();
            }

            return total;
        }

        public override int GetAggressionBonus()
        {
            int total = 0;

            foreach (Item item in slots)
            {
                total += item.GetAggressionBonus();
            }

            return total;
        }

        public override int GetWisdomBonus()
        {
            int total = 0;

            foreach (Item item in slots)
            {
                total += item.GetWisdomBonus();
            }

            return total;
        }
    }
}