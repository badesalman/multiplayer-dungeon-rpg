using ConOOD_RpgGame.World.Entities.Core.combat.Events;
using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Entities.Core.observers;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core
{
    public class SoundManager
    {
        private static readonly List<INoiseListener> listeners = new List<INoiseListener>();

        public void Subscribe(INoiseListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void Unsubscribe(INoiseListener listener)
        {
            listeners.Remove(listener);
        }


        public void BroadcastNoise(NoiseEvent noiseEvent)
        {
            GameLogger.Instance.Log(
                $"Noise broadcasted from {noiseEvent.SourceName} with range {noiseEvent.Range}.");

            foreach (INoiseListener listener in listeners.ToArray())
            {
                listener.OnNoiseHeard(noiseEvent);
            }
        }
    }
}