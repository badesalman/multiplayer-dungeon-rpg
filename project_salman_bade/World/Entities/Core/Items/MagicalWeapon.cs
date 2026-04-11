namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class MagicalWeapon : Weapon
    {
        protected override string WeaponTypeName => "Magical";
        public MagicalWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override int GetWeaponDamageFor(Player player)
        {
            return Damage
        + player.GetCurrentWeaponDamageBonus()
        + player.GetWisdom();
        }

        public override int GetNormalDefenseFor(Player player)
        {
            return player.GetDexterity() + player.GetLuck();
        }

        public override int GetStealthDefenseFor(Player player)
        {
            return 0;
        }

        public override int GetMagicalDefenseFor(Player player)
        {
            return player.GetWisdom() * 2;
        }
    }
}