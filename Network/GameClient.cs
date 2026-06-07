using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ConOOD_RpgGame.Network.DTO;
using ConOOD_RpgGame.Views;

namespace ConOOD_RpgGame.Network
{
    public class GameClient
    {

        private readonly string ip;
        private readonly int port;

        private int playerId;
        private StreamReader reader;
        private StreamWriter writer;
        private GameStateDto lastState;
        private readonly object stateLock = new object();

        private readonly ConsoleGameView view;
        private readonly object consoleLock = new object();
        private bool inMenu;
        public GameClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            view = new ConsoleGameView();
        }

        public void Start()
        {
            TcpClient client = new TcpClient();
            client.Connect(ip, port);

            NetworkStream stream = client.GetStream();

            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8);
            writer.AutoFlush = true;

            NetworkMessageDto welcome = JsonProtocol.Receive(reader);

            if (welcome != null)
            {
                playerId = welcome.PlayerId;

                if (welcome.State != null)
                {
                    view.Render(welcome.State);
                }
            }

            Task.Run(() => ListenForUpdates());

            ReadInputLoop();
        }

        private void ReadInputLoop()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                string action = "";

                if (key.Key == ConsoleKey.W)
                {
                    action = "UP";
                }
                else if (key.Key == ConsoleKey.S)
                {
                    action = "DOWN";
                }
                else if (key.Key == ConsoleKey.A)
                {
                    action = "LEFT";
                }
                else if (key.Key == ConsoleKey.D)
                {
                    action = "RIGHT";
                }
                else if (key.Key == ConsoleKey.E)
                {
                    action = "PICKUP";
                }
                else if (key.Key == ConsoleKey.I)
                {
                    OpenInventoryMenu();
                    continue;
                }
                else if (key.Key == ConsoleKey.D1)
                {
                    action = "ATTACK_NORMAL";
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    action = "ATTACK_STEALTH";
                }
                else if (key.Key == ConsoleKey.D3)
                {
                    action = "ATTACK_MAGICAL";
                }
                else if (key.Key == ConsoleKey.J)
                {
                    ShowJournalPage();
                    continue;
                }
                else if (key.Key == ConsoleKey.X)
                {
                    break;
                }

                if (action == "")
                {
                    continue;
                }

                SendAction(action);
            }
        }

        private void OpenInventoryMenu()
        {
            inMenu = true;

            SendAction("INVENTORY");

            System.Threading.Thread.Sleep(100);

            GameStateDto state;

            lock (stateLock)
            {
                state = lastState;
            }

            Console.Clear();
            Console.CursorVisible = true;

            if (state != null && !string.IsNullOrWhiteSpace(state.InventoryText))
            {
                Console.WriteLine(state.InventoryText);
            }
            else
            {
                Console.WriteLine("=== INVENTORY MENU ===");
                Console.WriteLine("(no inventory data)");
            }

            Console.WriteLine();
            Console.Write("Command: ");

            string input = Console.ReadLine();

            Console.CursorVisible = false;

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Trim().ToUpper();

                if (input != "Q")
                {
                    SendAction("INV_" + input);
                }
            }

            inMenu = false;

            lock (stateLock)
            {
                state = lastState;
            }

            if (state != null)
            {
                view.Render(state);
            }
        }

        private void SendAction(string action)
        {
            JsonProtocol.Send(writer, new NetworkMessageDto
            {
                Type = "ACTION",
                PlayerId = playerId,
                Action = action
            });
        }
        private void ListenForUpdates()
        {
            while (true)
            {
                NetworkMessageDto message = JsonProtocol.Receive(reader);

                if (message == null)
                {
                    break;
                }

                if (message.State != null)
                {
                    lock (stateLock)
                    {
                        lastState = message.State;
                    }

                    if (!inMenu)
                    {
                        view.Render(message.State);
                    }
                }
            }
        }
        private void ShowJournalPage()
        {
            inMenu = true;

            GameStateDto state;

            lock (stateLock)
            {
                state = lastState;
            }

            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("=== EVENT LOG ===");
            Console.WriteLine();

            if (state != null && state.RecentLogs != null)
            {
                foreach (string log in state.RecentLogs)
                {
                    Console.WriteLine(log);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey(true);

            Console.CursorVisible = false;
            inMenu = false;

            if (state != null)
            {
                view.Render(state);
            }
        }

    }
}