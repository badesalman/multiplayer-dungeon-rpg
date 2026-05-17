namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class LightWeapon : Weapon
    {
        protected override string WeaponTypeName => "Light";
        public override int NoiseRange => 2; // Light
        public LightWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override void Accept(IAttackVisitor visitor, Player player)
        {
            visitor.VisitLightWeapon(this, player);
        }
    }
}