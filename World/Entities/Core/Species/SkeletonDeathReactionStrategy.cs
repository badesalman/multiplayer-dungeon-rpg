using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Species
{
    public class SkeletonDeathReactionStrategy : IDeathReactionStrategy
    {
        public void React(Enemy enemy)
        {
            enemy.Attack += 2;
            enemy.Armor += 1;
        }
    }
}
