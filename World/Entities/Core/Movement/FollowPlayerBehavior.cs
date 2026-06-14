using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class FollowPlayerBehavior : IPlayerReactionBehavior
    {
        public void Move(Enemy enemy, World world, IEnumerable<Player> players)
        {
            Player target = enemy.FindNearestVisiblePlayer(players, world);

            if (target == null)
            {
                enemy.MoveBySoundOrDefault();
                return;
            }

            if (EnemyMovementHelper.Distance(enemy.Position, target.Position) == 1)
            {
                target.ReceiveDamage(enemy.Attack);
                return;
            }

            EnemyMovementHelper.MoveToward(enemy, world, target.Position);
        }
    }
}