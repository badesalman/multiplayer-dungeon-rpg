using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.combat.Events
{
    public class DeathEvent
    {
        public Position DeathPosition { get; }
        public string SpeciesName { get; }

        public DeathEvent(Position deathPosition, string speciesName)
        {
            DeathPosition = deathPosition;
            SpeciesName = speciesName;
        }
    }
}
