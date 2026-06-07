using ConOOD_RpgGame.Network;
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

            if (args.Length > 0 && args[0] == "--server")
            {
                int port = 5555;

                if (args.Length > 1)
                {
                    int.TryParse(args[1], out port);
                }

                GameServer server = new GameServer(port);
                server.Start();
                return;
            }

            if (args.Length > 0 && args[0] == "--client")
            {
                string ip = "127.0.0.1";
                int port = 5555;

                if (args.Length > 1)
                {
                    string[] parts = args[1].Split(':');
                    ip = parts[0];

                    if (parts.Length > 1)
                    {
                        int.TryParse(parts[1], out port);
                    }
                }

                GameClient client = new GameClient(ip, port);
                client.Start();
                return;
            }

            Console.WriteLine("Start as (S)erver or (C)lient?");
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.S)
            {
                GameServer server = new GameServer(5555);
                server.Start();
            }
            else if (key == ConsoleKey.C)
            {
                GameClient client = new GameClient("127.0.0.1", 5555);
                client.Start();
            }
            else
            {
                Console.WriteLine("Invalid option. Please restart and choose S or C.");
            }
        }
    }
}