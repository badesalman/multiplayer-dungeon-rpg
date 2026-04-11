using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class MagicalAttack : AttackStrategy
    {
        public override string Name => "Magical";

        public override int CalculateDamage(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return GetDefaultDamageForNonWeapon();
            }

            if (weapon is MagicalWeapon)
            {
                return weapon.GetWeaponDamageFor(player);
            }

            return 1;
        }

        public override int CalculateDefense(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return player.GetLuck();
            }

            return weapon.GetMagicalDefenseFor(player);
        }
    }
}