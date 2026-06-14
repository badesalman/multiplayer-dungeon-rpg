using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class RunAwayFromPlayerBehavior : IPlayerReactionBehavior
    {
        public void Move(Enemy enemy, World world, IEnumerable<Player> players)
        {
            Player target = enemy.FindNearestVisiblePlayer(players, world);

            if (target == null)
            {
                enemy.MoveBySoundOrDefault();
                return;
            }

            EnemyMovementHelper.MoveAwayFrom(enemy, world, target.Position);
        }
    }
}