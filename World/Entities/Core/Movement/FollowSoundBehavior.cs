using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class FollowSoundBehavior : ISoundReactionBehavior
    {
        public void Move(Enemy enemy, World world, Position soundPosition)
        {
            EnemyMovementHelper.MoveToward(enemy, world, soundPosition);

            if (EnemyMovementHelper.Distance(enemy.Position, soundPosition) == 0)
            {
                enemy.ClearSoundTarget();
            }
        }
    }
}