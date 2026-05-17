using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.combat.Events
{
    public class NoiseEvent
    {
        public Position SourcePosition { get; }
        public int Range { get; }
        public string SourceName { get; }

        public NoiseEvent(Position sourcePosition, int range, string sourceName)
        {
            SourcePosition = sourcePosition;
            Range = range;
            SourceName = sourceName;
        }
    }
}
