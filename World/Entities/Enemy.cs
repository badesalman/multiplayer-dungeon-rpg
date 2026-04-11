using ConOOD_RpgGame.World.Entities.Core;

namespace ConOOD_RpgGame.World.Entities
{
    public class Enemy : Character
    {
        public int Attack { get; }

        public override char Symbol => 'E';

        public Enemy(Position position, int health, int attack, int armor)
            : base(position, health, armor)
        {
            Attack = attack;
        }
    }
}