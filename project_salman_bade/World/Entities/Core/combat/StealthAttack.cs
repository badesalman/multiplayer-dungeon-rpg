using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class StealthAttack : AttackStrategy
    {
        public override string Name => "Stealth";

        public override int CalculateDamage(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return GetDefaultDamageForNonWeapon();
            }

            // LIGHT damage x2
            if (weapon is LightWeapon)
            {
                return weapon.GetWeaponDamageFor(player) * 2;
            }

            // HEAVYcsadece base damage /2
            if (weapon is HeavyWeapon)
            {
                int baseDamage = weapon.GetBaseDamage();
                int bonus = player.GetCurrentWeaponDamageBonus();

                return (baseDamage + bonus) / 2;
            }

            // MAGICAL her zaman 1
            if (weapon is MagicalWeapon)
            {
                return 1;
            }

            return 0;
        }

        public override int CalculateDefense(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return 0;
            }

            return weapon.GetStealthDefenseFor(player);
        }
    }
}