using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class ArtifactItem : Item
    {
        public ArtifactItem(string name, char symbol, string description)
            : base(name, symbol, description)
        {
        }

        public override string GetDetailedInfo()
        {
            return $"{Name} (Artifact) - {Description}";
        }
    }
}
