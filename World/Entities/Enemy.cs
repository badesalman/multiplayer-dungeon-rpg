using ConOOD_RpgGame.World.Entities.Core;

namespace ConOOD_RpgGame.World.Entities
{
    public class Enemy : Character
    {
        public int Attack { get; }

        public override char Symbol => 'E';
        public string Name { get; }

        public Enemy(Position position, int health, int attack, int armor, string name)
            : base(position, health, attack)
        {
            Armor = armor;
            Name = name;
            Attack = attack;
        }
       
    }
}