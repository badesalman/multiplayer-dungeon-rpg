using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public abstract class AttackStrategy
    {
        public abstract string Name { get; }

        public abstract int CalculateDamage(Player player, Weapon weapon);
        public abstract int CalculateDefense(Player player, Weapon weapon);

        protected int GetDefaultDamageForNonWeapon()
        {
            return 0;
        }
    }
}