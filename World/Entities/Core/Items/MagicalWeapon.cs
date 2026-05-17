namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class MagicalWeapon : Weapon
    {
        protected override string WeaponTypeName => "Magical";
        public override int NoiseRange => 5; // Magical
        public MagicalWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override void Accept(IAttackVisitor visitor, Player player)
        {
            visitor.VisitMagicalWeapon(this, player);
        }
    }
}