using ConOOD_RpgGame.World.Enums;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Modifiers
{
    public abstract class ItemModifier : Item
    {
        protected Item BaseItem { get; }

        protected ItemModifier(Item baseItem)
            : base(baseItem.Name, baseItem.Symbol, baseItem.Description)
        {
            BaseItem = baseItem;
        }

        protected abstract string ModifierName { get; }

        public override string GetDisplayText()
        {
            return $"{BaseItem.GetDisplayText()} ({ModifierName})";
        }

        public override string GetDetailedInfo()
        {
            return $"{BaseItem.GetDetailedInfo()} ({ModifierName})";
        }

        public override bool PickUp(Player player, out string message)
        {
            player.AddToInventory(this);
            message = $"{GetDisplayText()} added to inventory.";
            return true;
        }

        public override bool TryEquip(Player player, HandSide handSide, out string message)
        {
            return player.EquipItemDirectly(this, handSide, out message);
        }

        public override int GetStrengthBonus()
        {
            return BaseItem.GetStrengthBonus();
        }

        public override int GetDexterityBonus()
        {
            return BaseItem.GetDexterityBonus();
        }

        public override int GetHealthBonus()
        {
            return BaseItem.GetHealthBonus();
        }

        public override int GetLuckBonus()
        {
            return BaseItem.GetLuckBonus();
        }

        public override int GetAggressionBonus()
        {
            return BaseItem.GetAggressionBonus();
        }

        public override int GetWisdomBonus()
        {
            return BaseItem.GetWisdomBonus();
        }

        public override int GetDamageBonus()
        {
            return BaseItem.GetDamageBonus();
        }

        public override Weapon AsWeapon()
        {
            return BaseItem.AsWeapon();
        }
    }
}