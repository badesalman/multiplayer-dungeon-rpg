using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Logging
{
    public interface ILogger
    {
        void Log(string message);
        List<string> GetRecentLogs(int count);
    }
}
