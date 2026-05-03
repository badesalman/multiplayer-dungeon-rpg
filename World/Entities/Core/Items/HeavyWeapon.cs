namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class HeavyWeapon : Weapon
    {
        protected override string WeaponTypeName => "Heavy";
        public HeavyWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override void Accept(IAttackVisitor visitor, Player player)
        {
            visitor.VisitHeavyWeapon(this, player);
        }
    }
}