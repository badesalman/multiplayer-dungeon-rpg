using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class IgnorePlayerBehavior : IPlayerReactionBehavior
    {
        public void Move(Enemy enemy, World world, IEnumerable<Player> players)
        {
            enemy.MoveBySoundOrDefault();
        }
    }
}