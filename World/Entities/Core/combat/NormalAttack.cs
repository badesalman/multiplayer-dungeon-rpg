using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class NormalAttack : AttackStrategy
    {
        public override string Name => "Normal";

        public override int CalculateDamage(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return GetDefaultDamageForNonWeapon();
            }

            if (weapon is MagicalWeapon)
            {
                return 1;
            }

            return weapon.GetWeaponDamageFor(player);
        }

        public override int CalculateDefense(Player player, Weapon weapon)
        {
            if (weapon == null)
            {
                return player.GetDexterity();
            }

            return weapon.GetNormalDefenseFor(player);
        }
    }
}