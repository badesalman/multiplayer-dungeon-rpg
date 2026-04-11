namespace ConOOD_RpgGame.World.Entities.Core.Items.Modifiers
{
    public class UnluckyModifier : ItemModifier
    {
        public UnluckyModifier(Item baseItem) : base(baseItem)
        {
        }

        protected override string ModifierName => "Unlucky";

        public override int GetLuckBonus()
        {
            return BaseItem.GetLuckBonus() - 5;
        }
    }
}