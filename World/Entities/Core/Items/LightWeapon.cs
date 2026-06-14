namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class LightWeapon : Weapon
    {
        protected override string WeaponTypeName => "Light";
        public override int NoiseRange => 2;

        public LightWeapon(
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
            visitor.VisitLightWeapon(this, player);
        }
    }
}