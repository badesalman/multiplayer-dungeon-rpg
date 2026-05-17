using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public interface IMovementBehavior
    {
        void Move(Enemy enemy, World world);
    }
}
