using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class NormalAttack : AttackStrategy
    {
        public override string Name => "Normal";
        
        public override void VisitHeavyWeapon(HeavyWeapon weapon, Player player)
        {
            damage = weapon.GetFinalBaseDamage() + player.GetStrength() + player.GetAggression();
            defense = player.GetStrength() + player.GetLuck();
        }

        public override void VisitLightWeapon(LightWeapon weapon, Player player)
        {
            damage = weapon.GetFinalBaseDamage() + player.GetDexterity() + player.GetLuck();
            defense = player.GetDexterity() + player.GetLuck();
        }

        public override void VisitMagicalWeapon(MagicalWeapon weapon, Player player)
        {
            damage = 1;
            defense = player.GetDexterity() + player.GetLuck();
        }

        public override void VisitItem(Item item, Player player)
        {
            damage = 0;
            defense = player.GetDexterity();
        }
    }
}
//player selects attack
//the correct method otomatik olarak çağırılır 
//this is double-dispatch (core of visitor pattern)