namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class HeavyWeapon : Weapon
    {
        protected override string WeaponTypeName => "Heavy";
        public override int NoiseRange => 8;

        public HeavyWeapon(
            string name,
            char symbol,
            string description,
            int damage,
            bool isTwoHanded,
            int slotCount = 0)
            : base(name, symbol, description, damage, isTwoHanded, slotCount)
        {
        }

        public override void Accept(IAttackVisitor visitor, Player player)
        {
            visitor.VisitHeavyWeapon(this, player);
        }
    }
}