namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class PassiveItem : Item
    {
        private readonly int strength;
        private readonly int dexterity;
        private readonly int luck;
        private readonly int aggression;
        private readonly int wisdom;

        public PassiveItem(
            string name,
            char symbol,
            string description,
            int strength,
            int dexterity,
            int luck,
            int aggression,
            int wisdom)
            : base(name, symbol, description)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.luck = luck;
            this.aggression = aggression;
            this.wisdom = wisdom;
        }

        public override bool CanBePlacedInSlot()
        {
            return true;
        }

        public override int GetStrengthBonus()//for example stat bonus 
        {
            return strength;
        }

        public override int GetDexterityBonus()
        {
            return dexterity;
        }

        public override int GetLuckBonus()
        {
            return luck;
        }

        public override int GetAggressionBonus()
        {
            return aggression;
        }

        public override int GetWisdomBonus()
        {
            return wisdom;
        }

        public override string GetDetailedInfo()
        {
            return $"{Name} (Passive) - STR:{strength} DEX:{dexterity} LUCK:{luck} AGR:{aggression} WIS:{wisdom}";
        }
    }
}