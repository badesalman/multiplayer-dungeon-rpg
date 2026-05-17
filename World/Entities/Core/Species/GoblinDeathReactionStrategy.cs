using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Species
{
    public class GoblinDeathReactionStrategy : IDeathReactionStrategy
    {
        public void React(Enemy enemy)
        {
            enemy.Attack -= 2;

            if (enemy.Attack < 1)
            {
                enemy.Attack = 1;
            }

            enemy.Armor -= 1;

            if (enemy.Armor < 0)
            {
                enemy.Armor = 0;
            }
        }
    }
}
