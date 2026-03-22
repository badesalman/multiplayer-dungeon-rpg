using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame
{
    public interface IDungeonStrategy
    {

        void Configure(DungeonBuilder builder);
    }
}
