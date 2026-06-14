using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class RunAwayFromSoundBehavior : ISoundReactionBehavior
    {
        public void Move(Enemy enemy, World world, Position soundPosition)
        {
            EnemyMovementHelper.MoveAwayFrom(enemy, world, soundPosition);
        }
    }
}