using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class HolderItem : Item
    {
        private readonly List<Item> slots;
        private readonly int slotCount;

        public HolderItem(string name, char symbol, string description, int slotCount)
            : base(name, symbol, description)
        {
            this.slotCount = slotCount;
            slots = new List<Item>();
        }

        public override bool CanBePlacedInSlot()
        {
            return true;
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
                message = $"{item.Name} cannot be placed in a slot.";
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

        public override string GetDisplayText()
        {
            return $"{Name} (Holder {slots.Count}/{slotCount})";
        }

        public override string GetDetailedInfo()
        {
            return $"{Name} (Holder) - Slots: {slots.Count}/{slotCount}";
        }
    }
}