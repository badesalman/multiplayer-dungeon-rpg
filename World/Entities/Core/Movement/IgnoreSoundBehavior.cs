using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class IgnoreSoundBehavior : ISoundReactionBehavior
    {
        public void Move(Enemy enemy, World world, Position soundPosition)
        {
            enemy.MoveDefault();
        }
    }
}