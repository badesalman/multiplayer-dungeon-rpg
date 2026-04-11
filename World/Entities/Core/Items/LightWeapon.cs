namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class LightWeapon : Weapon
    {
        protected override string WeaponTypeName => "Light";

        public LightWeapon(string name, char symbol, string description, int damage, bool isTwoHanded)
            : base(name, symbol, description, damage, isTwoHanded)
        {
        }

        public override int GetWeaponDamageFor(Player player)
        {
            return Damage
                + player.GetCurrentWeaponDamageBonus()
                + player.GetDexterity()
                + player.GetLuck();
        }

        public override int GetNormalDefenseFor(Player player)
        {
            return player.GetDexterity() + player.GetLuck();
        }

        public override int GetStealthDefenseFor(Player player)
        {
            return player.GetDexterity();
        }

        public override int GetMagicalDefenseFor(Player player)
        {
            return player.GetLuck();
        }
    }
}