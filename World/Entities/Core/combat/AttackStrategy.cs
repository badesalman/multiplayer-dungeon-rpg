using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public abstract class AttackStrategy : IAttackVisitor
    {
        protected int damage;
        protected int defense;

        public int Damage => damage;
        public int Defense => defense;

        public abstract string Name { get; }

        public void Execute(Item item, Player player)
        {
            damage = 0;
            defense = 0;

            if (item == null)
            {
                VisitItem(null, player);
                return;
            }

            item.Accept(this, player);
        }


        //weapon subclasses
        public abstract void VisitHeavyWeapon(HeavyWeapon weapon, Player player);
        public abstract void VisitLightWeapon(LightWeapon weapon, Player player);
        public abstract void VisitMagicalWeapon(MagicalWeapon weapon, Player player);
        public abstract void VisitItem(Item item, Player player);
    }
}