using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class StealthAttack : AttackStrategy
    {
        public override string Name => "Stealth";

        public override void VisitHeavyWeapon(HeavyWeapon weapon, Player player)
        {
            damage = (player.GetStrength() + weapon.GetFinalBaseDamage()) / 2;
            defense = player.GetStrength();
        }

        public override void VisitLightWeapon(LightWeapon weapon, Player player)
        {
            damage = (player.GetDexterity() + weapon.GetFinalBaseDamage()) * 2;
            defense = player.GetDexterity();
        }

        public override void VisitMagicalWeapon(MagicalWeapon weapon, Player player)
        {
            damage = 1;
            defense = 0;
        }

        public override void VisitItem(Item item, Player player)
        {
            damage = 0;
            defense = 0;
        }
    }
}