using ConOOD_RpgGame.World.Entities.Core;

namespace ConOOD_RpgGame.World.Entities
{
    public abstract class Character : MapEntity, IAttackable
    {
        public int Health { get; protected set; }
        public int Armor { get; protected set; }

        public bool IsAlive => Health > 0;

        protected Character(Position position, int health, int armor)
            : base(position)
        {
            Health = health;
            Armor = armor;
        }

        public virtual void ReceiveDamage(int damage)
        {
            int finalDamage = damage - Armor;

            if (finalDamage < 0)
            {
                finalDamage = 0;
            }

            Health -= finalDamage;

            if (Health < 0)
            {
                Health = 0;
            }
        }
    }
}