using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConOOD_RpgGame.World.Entities.Core.Logging
{

    public class GameLogger : ILogger
    {
        private static GameLogger instance;
        private readonly List<string> logs;
        private readonly string filePath;

        private GameLogger(string playerName, string basePath)
        {
            logs = new List<string>();

            string fileName = $"{playerName}_{DateTime.Now:yyyyMMdd_HHmmss}.log";
            filePath = Path.Combine(basePath, fileName);

            Directory.CreateDirectory(basePath);
        }

        public static void Initialize(string playerName, string path)
        {
            if (instance == null)
                instance = new GameLogger(playerName, path);
        }

        public static GameLogger Instance => instance;

        public void Log(string message)
        {
            string entry = $"{DateTime.Now:HH:mm:ss} - {message}"; //buraya milisaniye mi eklemeliyimm?
            logs.Add(entry);

            File.AppendAllText(filePath, entry + Environment.NewLine);

            // limit memory (hocanın dediği)
            if (logs.Count > 50)
                logs.RemoveAt(0);
        }

        public List<string> GetRecentLogs(int count)
        {
            return logs.TakeLast(count).ToList();
        }

        public string GetLogFilePath()
        {
            return filePath;
        }

    }
}
//SİNGLETON 