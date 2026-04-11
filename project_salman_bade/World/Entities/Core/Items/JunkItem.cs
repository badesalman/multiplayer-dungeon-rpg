using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class JunkItem : Item
    {
        public JunkItem(string name, char symbol, string description)//cons
            : base(name, symbol, description)
        {
        }
    }
}
