using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.Logging;
using System;
using System.Text;

namespace ConOOD_RpgGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigLoader.Load("config.json");

            
            GameLogger.Initialize(config.PlayerName, config.LogFilePath);

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            Game game = new Game(config);
            game.Run();
        }
    }
}