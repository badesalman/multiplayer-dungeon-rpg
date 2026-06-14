namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class MagicalWeapon : Weapon
    {
        protected override string WeaponTypeName => "Magical";
        public override int NoiseRange => 5;

        public MagicalWeapon(
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
            visitor.VisitMagicalWeapon(this, player);
        }
    }
}