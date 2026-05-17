using ConOOD_RpgGame.World.Entities.Core.combat.Events;
using ConOOD_RpgGame.World.Entities.Core.observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Species
{
    public class SpeciesGroup
    {
        private readonly List<IDeathListener> listeners;

        public string Name { get; }

        public SpeciesGroup(string name)
        {
            Name = name;
            listeners = new List<IDeathListener>();
        }

        public void Subscribe(IDeathListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unsubscribe(IDeathListener listener)
        {
            listeners.Remove(listener);
        }

        public void BroadcastDeath(DeathEvent deathEvent)
        {
            foreach (IDeathListener listener in listeners.ToArray())
            {
                listener.OnDeathReceived(deathEvent);
            }
        }
    }
}

