using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public interface IPlayerReactionBehavior
    {
        void Move(Enemy enemy, World world, IEnumerable<Player> players);
    }
}