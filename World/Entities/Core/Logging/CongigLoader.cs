using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace ConOOD_RpgGame.World.Entities.Core.Logging
{
    public static class ConfigLoader
    {
        public static GameConfig Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GameConfig>(json);
        }
    }
}
