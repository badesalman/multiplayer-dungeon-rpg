using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConOOD_RpgGame.World.Entities.Core.combat.Events;

namespace ConOOD_RpgGame.World.Entities.Core.observers
{
    public interface IDeathListener
    {
        void OnDeathReceived(DeathEvent deathEvent);
    }
}