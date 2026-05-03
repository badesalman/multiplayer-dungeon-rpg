using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class MagicalAttack : AttackStrategy
    {
        public override string Name => "Magical";

        public override void VisitHeavyWeapon(HeavyWeapon weapon, Player player)
        {
            damage = 1;
            defense = player.GetLuck();
        }

        public override void VisitLightWeapon(LightWeapon weapon, Player player)
        {
            damage = 1;
            defense = player.GetLuck();
        }

        public override void VisitMagicalWeapon(MagicalWeapon weapon, Player player)
        {
            damage = player.GetWisdom() + weapon.GetFinalBaseDamage();
            defense = player.GetWisdom() * 2;
        }

        public override void VisitItem(Item item, Player player)
        {
            damage = 0;
            defense = player.GetLuck();
        }
    }
}