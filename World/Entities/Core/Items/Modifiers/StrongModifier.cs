namespace ConOOD_RpgGame.World.Entities.Core.Items.Modifiers
{
    public class StrongModifier : ItemModifier
    {
        public StrongModifier(Item baseItem) : base(baseItem)
        {
        }

        protected override string ModifierName => "Strong";

        public override int GetDamageBonus()
        {
            return BaseItem.GetDamageBonus() + 5;
        }
    }
}