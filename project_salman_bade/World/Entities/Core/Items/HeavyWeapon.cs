namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class HeavyWeapon : Weapon
    {
        protected override string WeaponTypeName => "Heavy";
        public HeavyWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override int GetWeaponDamageFor(Player player)
        {
            return GetFinalBaseDamage() + player.GetStrength() + player.GetAggression();
        }

        public override int GetNormalDefenseFor(Player player)
        {
            return player.GetStrength() + player.GetLuck();
        }

        public override int GetStealthDefenseFor(Player player)
        {
            return player.GetStrength();
        }

        public override int GetMagicalDefenseFor(Player player)
        {
            return player.GetLuck();
        }
    }
}